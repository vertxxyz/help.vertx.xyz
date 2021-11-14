using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Markdig;
using Markdig.Extensions.SelfPipeline;
using Markdig.Parsers;
using Troubleshooter.Constants;
using Troubleshooter.Renderers;

namespace Troubleshooter;

public enum ResourceType
{
	None,
	Markdown,
	RichText,
	Html
}

public enum ResourceLocation
{
	/// <summary>
	/// Content is embedded in other pages and is never root content in the output
	/// </summary>
	Embed,
	/// <summary>
	/// Content is root content in the html output
	/// </summary>
	Site
}

public class PageResource
{
	/// <summary>
	/// Full path to the source file
	/// </summary>
	public readonly string FullPath;

	/// <summary>
	/// What type of data this resource contains, markdown, rich text, etc.
	/// </summary>
	public readonly ResourceType Type;

	/// <summary>
	/// Where the resource is - site content, embedded page content, etc.
	/// </summary>
	public readonly ResourceLocation Location;

	/// <summary>
	/// Processed output html
	/// </summary>
	public string MarkdownText { get; set; }

	/// <summary>
	/// Processed output html
	/// </summary>
	public string HtmlText { get; private set; }

	// -------- Unbuilt resources --------
	/// <summary>
	/// Resources that are embedded into this page
	/// </summary>
	private HashSet<string> embedded;
	/// <summary>
	/// Resources that are embedded into other pages
	/// </summary>
	private HashSet<string> embeddedInto;
	// -----------------------------------

	/// <summary>
	/// Resources that are embedded into this page
	/// </summary>
	public HashSet<string> Embedded => embedded;
	/// <summary>
	/// Resources that are embedded into other pages
	/// </summary>
	public HashSet<string> EmbeddedInto => embeddedInto;

	public PageResource(string fullPath, ResourceType type, ResourceLocation location)
	{
		Type = type;
		Location = location;
		FullPath = fullPath;
	}

	public void AddEmbeddedInto(string page)
	{
		embeddedInto ??= new HashSet<string>();
		embeddedInto.Add(page);
	}

	public void AddEmbedded(string page)
	{
		embedded ??= new HashSet<string>();
		embedded.Add(page);
	}

	public void BuildText(Site site, PageResources allResources, MarkdownPipeline markdownPipeline)
	{
		switch (Type)
		{
			case ResourceType.None:
				SetHtmlTextAsEmpty();
				return;
			case ResourceType.Markdown:
				BuildMarkdown(site, allResources, markdownPipeline);
				break;
			case ResourceType.RichText:
				try
				{
					HtmlText = RtfUtility.RtfToHtml(File.ReadAllText(FullPath));
				}
				catch (Exception e)
				{
					throw new BuildException(e, $"Error when parsing \"{FullPath}\" into RTF");
				}
				break;
			case ResourceType.Html:
				try
				{
					HtmlText = HtmlUtility.Parse(File.ReadAllText(FullPath));
				}
				catch (Exception e)
				{
					throw new BuildException(e, $"Error when parsing HTML at \"{FullPath}\"");
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public void SetHtmlTextAsEmpty() => HtmlText = string.Empty;

	/// <summary>
	/// Builds page to <see cref="HtmlText"/> to be embedded in other content or written to disk
	/// </summary>
	private void BuildMarkdown(Site site, PageResources allResources, MarkdownPipeline markdownPipeline)
	{
		if (MarkdownText == null)
			ProcessMarkdown(File.ReadAllText(FullPath), site, allResources);
		HtmlText = Location switch
		{
			ResourceLocation.Embed =>
				// Embeds are not fully processed into HTML until they are built when embedded into site content.
				// This is done because something like Abbreviations requires the abbreviation target to be processed at the same time as the source.
				MarkdownText,
			ResourceLocation.Site => ToHtml(MarkdownText, markdownPipeline),
			_ => throw new ArgumentOutOfRangeException(nameof(Location), Location, "Location was not handled.")
		};
	}
		
	private static string ToHtml(string markdown, MarkdownPipeline pipeline)
	{
		string markdownPreProcessed = MarkdownPreProcessors.Process(markdown);
			
		MarkdownPipeline GetPipeline()
		{
			var selfPipeline = pipeline.Extensions.Find<SelfPipelineExtension>();
			return selfPipeline is not null ? selfPipeline.CreatePipelineFromInput(markdownPreProcessed) : pipeline;
		}
			
		pipeline = GetPipeline();
			
		var document = MarkdownParser.Parse(markdownPreProcessed, pipeline);
		using var rentedRenderer = pipeline.RentCustomHtmlRenderer();
		CustomHtmlRenderer renderer = rentedRenderer.Instance;
			
		renderer.Render(document);
		renderer.Writer.Flush();
		return HtmlPostProcessors.Process(renderer.Writer.ToString() ?? string.Empty);
	}

	public void ProcessMarkdown(string text, Site site, PageResources allResources)
	{
		string allText = text;
		StringBuilder stringBuilder = new StringBuilder();

		// Find and replace embed anchors with their content
		{
			var embeds = PageUtility.EmbedsAsLocalEmbedPaths(allText);

			int last = 0;
			foreach ((string localPath, Group group) in embeds)
			{
				string fullPath = PageUtility.LocalEmbedToFullPath(localPath, site);
				if (!allResources.TryGetValue(fullPath, out var embeddedPage))
					throw new LogicException($"\"{fullPath}\" is missing from {nameof(allResources)} while processing \"{FullPath}\".{nameof(embeds)}");

				stringBuilder.Append(allText[last..(group.Index - 2)]);
				stringBuilder.Append(embeddedPage.HtmlText);
				last = group.Index + group.Length + 2;
			}

			stringBuilder.Append(allText[last..]);
		}

		// Find and replace image links with root-based links
		{
			allText = stringBuilder.ToString();
			stringBuilder.Clear();

			int rootIndex;
			string directoryRoot; // The root output directory.
			string embedsDirectory = Path.Combine(Arguments.HtmlOutputDirectoryName, "Embeds").ToConsistentPath();
			switch (Location)
			{
				case ResourceLocation.Embed:
					rootIndex = site.EmbedRootIndex;
					directoryRoot = embedsDirectory;
					break;
				case ResourceLocation.Site:
					rootIndex = site.RootIndex;
					directoryRoot = Arguments.HtmlOutputDirectoryName;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			string directory = Path.GetDirectoryName(Site.FinalisePathWithRootIndex(FullPath, rootIndex));

			int last = 0;
			foreach ((string image, Group group) in PageUtility.LocalImagesAsRootPaths(allText))
			{
				if (group.Value.StartsWith(embedsDirectory))
					continue;

				stringBuilder.Append(allText[last..group.Index]);
				string imagePath = image.FinaliseDirectoryPathOnly(); // The path explicitly mentioned in the markdown
				string combinedPath = Path.Combine(directoryRoot, directory!, imagePath);
				string finalPath = HttpUtility.UrlPathEncode(combinedPath).ToConsistentPath();
				stringBuilder.Append(finalPath);
				last = group.Index + group.Length;
			}

			stringBuilder.Append(allText[last..]);
		}

		MarkdownText = stringBuilder.ToString();
	}

	public enum WriteStatus
	{
		Ignored,
		Skipped,
		Written
	}

	public WriteStatus WriteToDisk(Arguments arguments, Site site)
	{
		switch (Location)
		{
			case ResourceLocation.Site:
				break;
			default:
				// Do not write to disk if this is not a part of the site.
				return WriteStatus.Ignored;
		}

		// Check the previously built file to see whether it ought to be re-written.
		var path = Path.Combine(arguments.HtmlOutputDirectory, $"{site.ConvertFullSitePathToLinkPath(FullPath)}.html");
		return IOUtility.CreateFileIfDifferent(path, HtmlText) ? WriteStatus.Written : WriteStatus.Skipped;
	}
}