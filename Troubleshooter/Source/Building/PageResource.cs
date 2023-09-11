using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Markdig;
using Markdig.Extensions.SelfPipeline;
using Markdig.Parsers;
using Troubleshooter.Renderers;

namespace Troubleshooter;

public enum ResourceType
{
	None,
	Markdown,
	RichText,
	Html,
	Generator
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

/// <summary>
/// Full paths to Page Resources.
/// </summary>
public sealed class PageResourcesLookup : Dictionary<string, PageResource> { }

public sealed partial class PageResource
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
	/// Processed markdown ready for building.
	/// </summary>
	public string? MarkdownText { get; set; }

	/// <summary>
	/// Processed output html
	/// </summary>
	public string? HtmlText { get; private set; }

	/// <summary>
	/// Output location. This is only processed after <see cref="WriteToDisk"/> is called.
	/// </summary>
	public string? OutputLinkPath { get; private set; }

	// -------- Unbuilt resources --------
	/// <summary>
	/// Resources that are embedded into this page
	/// </summary>
	public HashSet<string>? Embedded { get; private set; }

	/// <summary>
	/// Resources that are embedded into other pages
	/// </summary>
	public HashSet<string>? EmbeddedInto { get; private set; }

	private string EmbedsDirectory =>
		_embedsDirectory ??= Path.Combine(Arguments.HtmlOutputDirectoryName, "Embeds").ToConsistentPath();
	
	private string ImagesDirectory =>
		_embedsDirectory ??= Path.Combine(Arguments.HtmlOutputDirectoryName, "Content", "Images").ToConsistentPath();

	private string? _embedsDirectory;

	public PageResource(string fullPath, ResourceType type, ResourceLocation location)
	{
		Type = type;
		Location = location;
		FullPath = fullPath;
	}

	public void AddEmbeddedInto(string page)
	{
		EmbeddedInto ??= new HashSet<string>();
		EmbeddedInto.Add(page);
	}

	public void AddEmbedded(string page)
	{
		Embedded ??= new HashSet<string>();
		Embedded.Add(page);
	}

	public void BuildText(Site site, PageResourcesLookup allResources, MarkdownPipeline markdownPipeline, MarkdownPreProcessors preProcessors, HtmlPostProcessors postProcessors)
	{
		switch (Type)
		{
			case ResourceType.None:
				SetHtmlTextAsEmpty();
				return;
			case ResourceType.Markdown:
				BuildMarkdownToHtml(site, allResources, markdownPipeline, preProcessors, postProcessors);
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
			case ResourceType.Generator:
				// Generators are processed during the gather stage.
				return;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public void SetHtmlTextAsEmpty() => HtmlText = string.Empty;

	/// <summary>
	/// Builds page to <see cref="HtmlText"/> to be embedded in other content or written to disk.
	/// </summary>
	private void BuildMarkdownToHtml(Site site, PageResourcesLookup allResources, MarkdownPipeline markdownPipeline, MarkdownPreProcessors preProcessors, HtmlPostProcessors postProcessors)
	{
		if (MarkdownText == null)
			ProcessMarkdown(File.ReadAllText(FullPath), site, allResources);

		string text = Location switch
		{
			ResourceLocation.Embed =>
				// Embeds are not fully processed into HTML until they are built when embedded into site content.
				// This is done because something like Abbreviations requires the abbreviation target to be processed at the same time as the source.
				MarkdownText!,
			ResourceLocation.Site => ToHtml(MarkdownText!, markdownPipeline, preProcessors, postProcessors, FullPath),
			_ => throw new ArgumentOutOfRangeException(nameof(Location), Location, "Location was not handled.")
		};

		if (HtmlText == null)
			HtmlText = text;
		else
			HtmlText += text;
	}

	private static string ToHtml(string markdown, MarkdownPipeline pipeline, MarkdownPreProcessors preProcessors, HtmlPostProcessors postProcessors, string fullPath)
	{
		string markdownPreProcessed = preProcessors.Process(markdown);

		pipeline = GetPipeline();

		var document = MarkdownParser.Parse(markdownPreProcessed, pipeline);
		using var rentedRenderer = pipeline.RentCustomHtmlRenderer();
		CustomHtmlRenderer renderer = rentedRenderer.Instance;

		renderer.Render(document);
		renderer.Writer.Flush();
		return postProcessors.Process(renderer.Writer.ToString() ?? string.Empty, fullPath);

		MarkdownPipeline GetPipeline()
		{
			var selfPipeline = pipeline.Extensions.Find<SelfPipelineExtension>();
			return selfPipeline is not null ? selfPipeline.CreatePipelineFromInput(markdownPreProcessed) : pipeline;
		}
	}

	public void ProcessMarkdown(string text, Site site, PageResourcesLookup? allResources)
	{
		string allText = text;
		StringBuilder stringBuilder = new();

		// Find and replace embed anchors with their content
		{
			var embeds = PageUtility.EmbedsAsLocalEmbedPaths(allText);

			int last = 0;
			foreach ((string localPath, Group group) in embeds)
			{
				if (allResources == null)
					throw new ArgumentException($"{nameof(allResources)} was null, and yet embeds were found.");
				string fullPath = PageUtility.LocalEmbedToFullPath(localPath, site);
				if (!allResources.TryGetValue(fullPath, out var embeddedPage))
					throw new LogicException(
						$"\"{fullPath}\" is missing from {nameof(allResources)} while processing \"{FullPath}\".{nameof(embeds)}");

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
			switch (Location)
			{
				case ResourceLocation.Embed:
					rootIndex = site.EmbedRootIndex;
					directoryRoot = EmbedsDirectory;
					break;
				case ResourceLocation.Site:
					rootIndex = site.RootIndex;
					directoryRoot = Arguments.HtmlOutputDirectoryName;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			string directory = Path.GetDirectoryName(Site.FinalisePathWithRootIndex(FullPath, rootIndex))!;

			int last = 0;
			foreach ((string image, Group group) in PageUtility.LocalImagesAsRootPaths(allText, false))
			{
				if (ApproximatelyStartsWith(group.Value, EmbedsDirectory, 2))
					continue;
				
				if (group.Value.StartsWith("/Images/"))
					continue;

				stringBuilder.Append(allText[last..group.Index]);
				string imagePath = image.FinaliseDirectoryPathOnly(); // The path explicitly mentioned in the markdown
				string combinedPath = Path.Combine(directoryRoot, directory, imagePath);
				string finalPath = HttpUtility.UrlPathEncode(combinedPath).ToConsistentPath();
				if (finalPath[0] != '/')
					stringBuilder.Append($"/{finalPath}");
				else
					stringBuilder.Append(finalPath);
				last = group.Index + group.Length;
			}

			stringBuilder.Append(allText[last..]);
		}

		MarkdownText = stringBuilder.ToString();
	}

	/// <summary>
	/// Check <see cref="count"/> positions at the start of <see cref="input"/> to see if <see cref="query"/> is contained there.
	/// </summary>
	private static bool ApproximatelyStartsWith(string input, string query, int count)
	{
		for (int i = 0; i < count; i++)
		{
			if (i + query.Length > input.Length)
				continue;
			ReadOnlySpan<char> span = input.AsSpan(i, query.Length);
			if (span.SequenceEqual(query)) return true;
		}

		return false;
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
			case ResourceLocation.Embed:
			default:
				// Do not write to disk if this is not a part of the site.
				return WriteStatus.Ignored;
		}

		if (Type == ResourceType.Generator)
		{
			// Generators do not write, they only create other resources.
			return WriteStatus.Ignored;
		}


		// Check the previously built file to see whether it ought to be re-written.
		OutputLinkPath ??= site.ConvertFullSitePathToLinkPath(FullPath);

		OutputLinkPath = s_NumberRegex.Replace(OutputLinkPath, "/");
		
		string path = Path.Combine(arguments.HtmlOutputDirectory!, $"{OutputLinkPath}.html");
		return IOUtility.CreateFileIfDifferent(path, HtmlText!) ? WriteStatus.Written : WriteStatus.Skipped;
	}

	private static readonly Regex s_NumberRegex = GetNumberRegex();
	
	[GeneratedRegex(@"/\d+-", RegexOptions.Compiled)]
	private static partial Regex GetNumberRegex();
}