using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

[Flags]
public enum ResourceFlags
{
	None,
	Symlink = 1 << 0,
	ExistsInOutput = 1 << 1,
	IndexPage = 1 << 2
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
/// Full paths (C:\...) to Page Resources.
/// </summary>
public sealed class PageResourcesLookup : Dictionary<string, PageResource>;

public sealed partial class PageResource
{
	/// <summary>
	/// Full path to the source file
	/// </summary>
	public readonly string FullPath;

	/// <summary>
	/// Full path to the source file if <see cref="ResourceFlags.Symlink"/> is true.
	/// </summary>
	public readonly string? SymlinkFullPath;

	/// <summary>
	/// What type of data this resource contains, markdown, rich text, etc.
	/// </summary>
	public readonly ResourceType Type;

	/// <summary>
	/// Specific info about this resource.
	/// </summary>
	public ResourceFlags Flags { get; private set; }

	/// <summary>
	/// Where the resource is - site content, embedded page content, etc.
	/// </summary>
	public readonly ResourceLocation Location;

	/// <summary>
	/// Processed markdown ready for building.
	/// </summary>
	public string? MarkdownText { get; set; }

	/// <summary>
	/// Processed output html used within the head block.
	/// </summary>
	public string? HeadHtmlText { get; private set; }

	/// <summary>
	/// Processed output html used within the body block.
	/// </summary>
	public string? BodyHtmlText { get; private set; }

	/// <summary>
	/// Output url.
	/// </summary>
	public string OutputLink { get; }

	/// <summary>
	/// Output file location. The file may not exist if <see cref="Flags"/> does not contain <see cref="ResourceFlags.ExistsInOutput"/>.
	/// </summary>
	public string OutputFilePath { get; }

	// -------- Unbuilt resources --------
	/// <summary>
	/// Resources that are embedded into this page
	/// </summary>
	public HashSet<string>? Embedded { get; private set; }

	/// <summary>
	/// Resources that are embedded into other pages
	/// </summary>
	public HashSet<string>? EmbeddedInto { get; private set; }

	/// <summary>
	/// An associated sidebar page (if one exists).
	/// </summary>
	public PageResource? Sidebar { get; set; }

	private List<PageResource>? _generatedChildren;

	public bool HasGeneratedChildren => _generatedChildren?.Count > 0;

	public IEnumerable<PageResource> GeneratedChildren => _generatedChildren ?? Enumerable.Empty<PageResource>();
	public bool IsSidebar { get; set; }

	public PageResource(
		string fullPath,
		ResourceType type,
		ResourceLocation location,
		string? symlinkTarget,
		string outputDirectory,
		Site site
	)
	{
		Type = type;
		Location = location;
		FullPath = fullPath;
		SymlinkFullPath = symlinkTarget;
		Flags = symlinkTarget != null ? ResourceFlags.Symlink : ResourceFlags.None;
		OutputLink ??= site.ConvertFullSitePathToLinkPath(FullPath);
		OutputLink = s_numberRegex.Replace(OutputLink, "/");
		OutputFilePath = Path.Combine(outputDirectory, $"{OutputLink}.html").ToWorkingPath();
	}

	public void AddGeneratedChild(PageResource pageResource) => (_generatedChildren ??= new List<PageResource>()).Add(pageResource);

	public void MarkAsIndexPage() => Flags |= ResourceFlags.IndexPage;

	public void AddEmbeddedInto(string page)
	{
		EmbeddedInto ??= [];
		EmbeddedInto.Add(page);
	}

	public void AddEmbedded(string page)
	{
		Embedded ??= [];
		Embedded.Add(page);
	}

	public void BuildText(
		Site site,
		PageResourcesLookup allResources,
		MarkdownPipeline markdownPipeline,
		IProcessorGroup processors
	)
	{
		// Symlinks use the data built in their originating files.
		if ((Flags & ResourceFlags.Symlink) != 0)
			return;

		switch (Type)
		{
			case ResourceType.None:
				SetHtmlTextAsEmpty();
				return;
			case ResourceType.Markdown:
				BuildMarkdownToHtml(site, allResources, markdownPipeline, processors);
				break;
			case ResourceType.RichText:
				try
				{
					BodyHtmlText = RtfUtility.RtfToHtml(File.ReadAllText(FullPath));
				}
				catch (Exception e)
				{
					throw new BuildException(e, $"Error when parsing \"{FullPath}\" into RTF");
				}

				break;
			case ResourceType.Html:
				try
				{
					BodyHtmlText = HtmlUtility.Parse(File.ReadAllText(FullPath));
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
				throw new ArgumentOutOfRangeException(nameof(Type), "Missing when building text.");
		}
	}

	public void SetHtmlTextAsEmpty() => BodyHtmlText = string.Empty;

	/// <summary>
	/// Builds page to <see cref="BodyHtmlText"/> to be embedded in other content or written to disk.
	/// </summary>
	private void BuildMarkdownToHtml(
		Site site,
		PageResourcesLookup allResources,
		MarkdownPipeline markdownPipeline,
		IProcessorGroup processors
	)
	{
		if (MarkdownText == null)
			ProcessMarkdown(File.ReadAllText(FullPath), site, allResources);

		(string? head, string text) = Location switch
		{
			ResourceLocation.Embed =>
				// Embeds are not fully processed into HTML until they are built when embedded into site content.
				// This is done because something like Abbreviations requires the abbreviation target to be processed at the same time as the source.
				(null, MarkdownText!),
			ResourceLocation.Site => ToHtml(site, markdownPipeline, processors),
			_ => throw new ArgumentOutOfRangeException(nameof(Location), Location, "Location was not handled.")
		};

		if (head != null)
		{
			if (HeadHtmlText == null)
				HeadHtmlText = head;
			else
				HeadHtmlText += head;
		}

		if (BodyHtmlText == null)
			BodyHtmlText = text;
		else
			BodyHtmlText += text;
	}

	private (string? head, string body) ToHtml(
		Site site,
		MarkdownPipeline pipeline,
		IProcessorGroup processors
	)
	{
		string markdownPreProcessed = processors.PreProcessors.Process(MarkdownText!);

		pipeline = GetPipeline();

		var document = MarkdownParser.Parse(markdownPreProcessed, pipeline);
		using var rendererScope = pipeline.RentCustomHtmlRenderer();
		CustomHtmlRenderer renderer = rendererScope.Instance;
		renderer.Site = site;
		renderer.CurrentResource = this;
		renderer.Render(document);
		var html = renderer.ToHtml();
		return
		(
			html.head,
			processors.PostProcessors.Process(html.body, FullPath)
		);

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
			var embeds = PageUtility.GetEmbedsAsLocalPathsFromMarkdownText(allText);

			int last = 0;
			foreach ((string localPath, Group group) in embeds)
			{
				if (allResources == null)
					throw new ArgumentException($"{nameof(allResources)} was null, and yet embeds were found.");
				string fullPath = PageUtility.GetFullPathFromLocalEmbed(localPath, site);
				if (!allResources.TryGetValue(fullPath, out var embeddedPage))
					throw new LogicException(
						$"\"{fullPath}\" is missing from {nameof(allResources)} while processing \"{FullPath}\".{nameof(embeds)}"
					);

				stringBuilder.Append(allText[last..(group.Index - 2)]);
				stringBuilder.Append(embeddedPage.BodyHtmlText);
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
					directoryRoot = PathUtility.EmbedsDirectory;
					break;
				case ResourceLocation.Site:
					rootIndex = site.RootIndex;
					directoryRoot = "";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			string directory = Path.GetDirectoryName(Site.FinalisePathWithRootIndex(FullPath, rootIndex))!;

			int last = 0;
			foreach ((string image, Group group) in PageUtility.GetImagesAsLocalPathsFromMarkdownText(allText, false))
			{
				if (ApproximatelyStartsWith(group.Value, PathUtility.EmbedsDirectoryLink, 2))
					continue;

				if (group.Value.StartsWith("/Images/"))
					continue;

				stringBuilder.Append(allText[last..group.Index]);
				string imagePath = image.ToFinalisedWorkingPathDirectoryOnly(); // The path explicitly mentioned in the markdown
				string combinedPath = Path.Combine(directoryRoot, directory, imagePath);
				string finalPath = HttpUtility.UrlPathEncode(combinedPath).ToOutputPath();
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

	public WriteStatus WriteToDisk(IOUtility.RecordType recordType = IOUtility.RecordType.Normal)
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

		if (IsSidebar)
		{
			// Sidebars are embedded in other pages, we don't write them to disc.
			return WriteStatus.Ignored;
		}

		if ((Flags & ResourceFlags.Symlink) != 0)
		{
			// Symlinks move other resources.
			return WriteStatus.Ignored;
		}

		if (Type is ResourceType.Generator)
		{
			// Generators do not write, they only create other resources.
			return WriteStatus.Ignored;
		}

		// The file is written/exists at this point.
		Flags |= ResourceFlags.ExistsInOutput;

		// Check the previously built file to see whether it ought to be re-written.
		return IOUtility.CreateFileIfDifferent(
			OutputFilePath,
			IndexHtml.Create(
				HeadHtmlText,
				BodyHtmlText!,
				Sidebar?.BodyHtmlText
			),
			recordType
		)
			? WriteStatus.Written
			: WriteStatus.Skipped;
	}

	private static readonly Regex s_numberRegex = GetNumberRegex();

	[GeneratedRegex(@"/\d+-")]
	private static partial Regex GetNumberRegex();
}
