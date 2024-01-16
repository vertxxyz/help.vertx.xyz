using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using Troubleshooter.Issues;
using static Troubleshooter.PageUtility;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	private static void BuildPages(
		Arguments arguments,
		Site site,
		MarkdownPipeline pipeline,
		IProcessorGroup processors
	)
	{
		var allResources = CollectPages(arguments, site);
		processors.ResourceProcessors.Process(allResources, arguments, site);
		var allBuiltResources = new HashSet<string>();

		arguments.VerboseLog($"{allResources.Count} total un-processed pages");

		int builtPages = 0, skippedPages = 0, ignoredPages = 0;

		while (allBuiltResources.Count != allResources.Count)
		{
			int prevBuilt = allBuiltResources.Count;
			foreach ((string path, PageResource resource) in allResources)
			{
				// Do not re-build already built content
				if (allBuiltResources.Contains(path))
					continue;

				// If this page has a sidebar page that's not yet built, then we cannot safely build it.
				if (resource.Sidebar != null && !allBuiltResources.Contains(resource.Sidebar.FullPath))
					continue;

				// If this page has something un-built embedded in it then we cannot safely build it.
				if (!resource.Embedded?.IsSubsetOf(allBuiltResources) ?? false)
					continue;

				// If the resource isn't in the site, and isn't embedded into anything, we can skip building it entirely.
				if (resource.Location == ResourceLocation.Embed && (resource.EmbeddedInto?.Count ?? 0) == 0)
				{
					arguments.VerboseLog($"\"{resource.FullPath}\" is not used.");
					resource.SetHtmlTextAsEmpty();
					allBuiltResources.Add(path);
					continue;
				}

				try
				{
					resource.BuildText(site, allResources, pipeline, processors);
				}
				catch (Exception e)
				{
					throw new BuildException(e, $"{resource.FullPath} failed to build text.");
				}

				allBuiltResources.Add(path);
				switch (resource.WriteToDisk())
				{
					case PageResource.WriteStatus.Ignored:
						ignoredPages++;
						break;
					case PageResource.WriteStatus.Skipped:
						skippedPages++;
						break;
					case PageResource.WriteStatus.Written:
						builtPages++;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			if (prevBuilt != allBuiltResources.Count)
				continue;

			var remainingResources = allResources.Select(r => r.Key).Where(p => !allBuiltResources.Contains(p));
			throw new BuildException($"Build has soft-locked - infinite loop due to recursive embedding, or non-existent embed?\nRemaining resources: \n{remainingResources.ToElementsString(p => $"- \"{p}\"")}");
		}

		processors.BuildPostProcessors.Process(arguments, allResources, site);

		arguments.VerboseLog($"{builtPages} pages written to disk. {skippedPages} were skipped as identical, and {ignoredPages} were embeds.");

		SourceIndex.GeneratePageSourceLookup(arguments, allResources);
	}

	private static PageResourcesLookup CollectPages(Arguments arguments, Site site)
	{
		string outputDirectory = arguments.Path;

		PageResourcesLookup pages = new();

		// Collect Embedded Pages
		foreach (string path in Directory.EnumerateFiles(site.EmbedsDirectory, "*", SearchOption.AllDirectories))
			DoProcessPath(path, ResourceLocation.Embed, null);

		// Collect Site Pages
		foreach (string path in Directory.EnumerateFiles(site.Directory, "*", SearchOption.AllDirectories).Where(p => p.EndsWith(".md") || p.EndsWith(".gen")))
			DoProcessPath(path, ResourceLocation.Site, null);

		// Collect symlink directories so files within those directories can be marked as links.
		foreach ((string fromInRedirectDirectory, string to) in CollectSymlinkedFilesLookup(site.RedirectsDirectory))
		{
			string from = $"{site.Directory}{fromInRedirectDirectory[site.RedirectsDirectory.Length..]}";
			DoProcessPath(from, ResourceLocation.Site, to);
		}

		CollectGeneratedPages();

		CollectAssociatedSidebarPages();

		return pages;

		void DoProcessPath(string path, ResourceLocation location, string? symlinkTo)
		{
			string fullPath = Path.GetFullPath(path);
			if (!TryGetNewResource(fullPath, location, out var page, symlinkTo))
				return;

			if (page!.Type != ResourceType.Markdown)
				return;

			string realSourceFile = symlinkTo ?? fullPath;

			if (!File.Exists(realSourceFile))
				throw new BuildException($"\"{realSourceFile}\" was not found. Likely a symlink-related bug.");

			// Collect all the links from this markdown page - and add them to the PageResources.
			string text = File.ReadAllText(realSourceFile);
			foreach (var embed in GetEmbedsAsLocalPathsFromMarkdownText(text))
			{
				string fullEmbedPath = GetFullPathFromLocalEmbed(embed.localPath, site);
				page.AddEmbedded(fullEmbedPath);
				if (!TryGetNewResource(fullEmbedPath, ResourceLocation.Embed, out PageResource? embeddedPage))
					continue;
				embeddedPage!.AddEmbeddedInto(fullPath);
			}
		}

		// Returns true if a page will be created for the path.
		bool TryGetNewResource(string fullPath, ResourceLocation location, out PageResource? page, string? symlinkTo = null)
		{
			if (pages.TryGetValue(fullPath, out page))
				return true;

			string extension = Path.GetExtension(fullPath);
			switch (extension)
			{
				case ".md" when fullPath.EndsWith(Constants.GeneratorSuffix):
					page = new PageResource(fullPath, ResourceType.Generator, location, symlinkTo, outputDirectory, site);
					break;
				case ".md":
					page = new PageResource(fullPath, ResourceType.Markdown, location, symlinkTo, outputDirectory, site);
					break;
				case ".gen":
					page = new PageResource(fullPath, ResourceType.Generator, location, symlinkTo, outputDirectory, site);
					break;
				case ".rtf":
					page = new PageResource(fullPath, ResourceType.RichText, location, symlinkTo, outputDirectory, site);
					break;
				case ".html":
				case ".nomnoml":
					page = new PageResource(fullPath, ResourceType.Html, location, symlinkTo, outputDirectory, site);
					break;
				default:
					// Ignore content that is not buildable page content.
					page = null;
					return false;
			}

			pages.Add(fullPath, page);
			return true;
		}

		void CollectGeneratedPages()
		{
			PageResourcesLookup toAppend = new();
			foreach ((_, PageResource page) in pages)
			{
				foreach ((string key, PageResource value) in ProcessGenerators(outputDirectory, site, pages, page))
				{
					toAppend.Add(key, value);
					page.AddGeneratedChild(value);
				}
			}

			foreach (KeyValuePair<string, PageResource> pair in toAppend)
				pages.Add(pair.Key, pair.Value);
		}

		void CollectAssociatedSidebarPages()
		{
			foreach ((string key, PageResource page) in pages)
			{
				if (page.Location == ResourceLocation.Embed)
					continue;

				if (key.EndsWith(Constants.SidebarGeneratorSuffix, StringComparison.Ordinal))
				{
					foreach (PageResource child in page.GeneratedChildren)
					{
						string childKey = child.FullPath;
						string contentPagePath = $"{childKey.Remove(childKey.Length - Constants.SidebarSuffix.Length)}.md";
						if (!pages.TryGetValue(contentPagePath, out var contentPage))
							throw new BuildException($"Generated sidebar page: \"{childKey}\" (source: \"{key}\") did not have associated content page at \"{contentPagePath}\".");

						contentPage.Sidebar = child;
						child.IsSidebar = true;
					}

					page.IsSidebar = true;
				}
				else if (key.EndsWith(Constants.SidebarSuffix, StringComparison.Ordinal))
				{
					string contentPagePath = $"{key.Remove(key.Length - Constants.SidebarSuffix.Length)}.md";
					if (!pages.TryGetValue(contentPagePath, out var contentPage))
						throw new BuildException($"Sidebar page: \"{key}\" did not have associated content page at \"{contentPagePath}\".");

					contentPage.Sidebar = page;
					page.IsSidebar = true;
				}
			}
		}
	}

	[GeneratedRegex(@"\[(.+?)\]\(([\w%/-]+?)\.md\)", RegexOptions.Compiled)]
	private static partial Regex GeneratorLinkRegex();

	[GeneratedRegex(@"//\s*bypass\s*$")]
	private static partial Regex BypassRegex();

	public static IEnumerable<(string key, PageResource value)> ProcessGenerators(string outputDirectory, Site site, PageResourcesLookup? allResources, PageResource page)
	{
		if ((page.Flags & ResourceFlags.Symlink) != 0)
			yield break;

		// Generated markdown
		if (page.FullPath.EndsWith(Constants.SidebarGeneratorSuffix))
		{
			foreach ((string key, PageResource value) valueTuple in GenerateSidebarPagesMarkdown())
				yield return valueTuple;
		}

		yield break;

		IEnumerable<(string key, PageResource value)> GenerateSidebarPagesMarkdown()
		{
			// Sidebar markdown
			page.ProcessMarkdown(File.ReadAllText(page.FullPath), site, allResources);

			// Links to a specific page are stripped, and the link text is bolded.
			// After processing, append the newly generated content to any pre-existing sidebar content, or create new.

			/*
			 *- [Script name](Script%20Name.md)
			 *- [Console errors](Console%20Errors.md)
			 *- [Base type](Base%20Type.md)
			 *- [Editor contexts](Editor%20Contexts.md)
			 *  - [Editor folders](Editor%20Folders.md)
			 *  - [Editor Assembly Definitions](Assembly%20Definitions.md)
			 *- [General advice](General%20Advice.md)
			 *- [Project reimport](Project%20Reimport.md)
			 */
			// E:\Projects\help.vertx.xyz\Troubleshooter\Assets\Site\Programming\Scripts\Loading\Script Loading_sidebar_generator.gen

			// You can avoid adding the generated content to a sidebar match by adding: "// bypass"

			string markdown = page.MarkdownText!;
			MatchCollection matches = GeneratorLinkRegex().Matches(markdown);
			string directory = Path.GetDirectoryName(page.FullPath)!;
			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];
				string relativeLink = match.Groups[2].Value;
				string path = $"{Path.Combine(directory, relativeLink)}{Constants.SidebarSuffix}";

				if (BypassRegex().IsMatch(StringUtility.LineAt(markdown, match.Index + match.Length)))
					continue;

				// Replace the link to *this* page with a bolded version
				string markdownText = markdown.Replace($"[{match.Groups[1].Value}]({relativeLink}.md)", $"**{match.Groups[1].Value}**");
				// Edit links to be local to the destination if required.
				if (relativeLink.Contains('/'))
				{
					string localDirectory = Path.GetDirectoryName(path)!;
					for (int j = 0; j < matches.Count; j++)
					{
						// Do not edit *this*, as it has been bolded already.
						if (i == j) continue;
						string originalPath = $"{Path.Combine(directory, matches[j].Groups[2].Value)}.md";
						string relativePath = Path.GetRelativePath(localDirectory, originalPath).Replace('\\', '/');
						markdownText = markdownText.Replace($"({matches[j].Groups[2].Value}.md)", $"({relativePath})");
					}
				}

				markdownText = markdownText.Replace(" // bypass", "");
				path = path.ToWorkingPath().ToUnTokenized();
				PageResource resource = new(path, ResourceType.Markdown, page.Location, page.SymlinkFullPath, outputDirectory, site) { MarkdownText = markdownText };
				yield return (path, resource);
			}
		}
	}
}
