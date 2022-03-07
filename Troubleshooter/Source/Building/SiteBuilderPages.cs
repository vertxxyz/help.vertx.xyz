using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using Troubleshooter.Constants;
using Troubleshooter.Issues;
using static Troubleshooter.PageUtility;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	private static void BuildPages(Arguments arguments, Site site, MarkdownPipeline pipeline)
	{
		var allResources = CollectPages(site);
		PageResourcesPostProcessors.Process(allResources, site);
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
					resource.BuildText(site, allResources, pipeline);
				}
				catch (Exception e)
				{
					throw new BuildException(e, $"{resource.FullPath} failed to build text.");
				}

				allBuiltResources.Add(path);
				switch (resource.WriteToDisk(arguments, site))
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
				}
			}

			if (prevBuilt == allBuiltResources.Count)
				throw new BuildException("Build has soft-locked - infinite loop due to recursive embedding?");
		}

		arguments.VerboseLog($"{builtPages} pages written to disk. {skippedPages} were skipped as identical, and {ignoredPages} were embeds.");
		
		SourceIndex.GeneratePageSourceLookup(arguments, allResources);
	}

	private static PageResources CollectPages(Site site)
	{
		PageResources pages = new();

		// Collect Embedded Pages
		foreach (var path in Directory.EnumerateFiles(site.EmbedsDirectory, "*", SearchOption.AllDirectories))
			DoProcessPath(path, ResourceLocation.Embed);

		// Collect Site Pages
		foreach (var path in Directory.EnumerateFiles(site.Directory, "*", SearchOption.AllDirectories).Where(p => p.EndsWith(".md") || p.EndsWith(".gen")))
			DoProcessPath(path, ResourceLocation.Site);

		CollectGeneratedPages();

		return pages;

		void DoProcessPath(string path, ResourceLocation location)
		{
			string fullPath = Path.GetFullPath(path);
			if (!TryGetNewResource(fullPath, location, out var page))
				return;

			if (page!.Type != ResourceType.Markdown)
				return;

			// Collect all the links from this markdown page - and add them to the PageResources.
			string text = File.ReadAllText(fullPath);
			foreach (var embed in EmbedsAsLocalEmbedPaths(text))
			{
				string fullEmbedPath = LocalEmbedToFullPath(embed.localPath, site);
				page.AddEmbedded(fullEmbedPath);
				if (!TryGetNewResource(fullEmbedPath, ResourceLocation.Embed, out PageResource? embeddedPage))
					continue;
				embeddedPage!.AddEmbeddedInto(fullPath);
			}
		}

		//Returns true if a new page is created
		bool TryGetNewResource(string fullPath, ResourceLocation location, out PageResource? page)
		{
			if (pages.TryGetValue(fullPath, out page))
				return true;

			string extension = Path.GetExtension(fullPath);
			switch (extension)
			{
				case ".md":
					page = new PageResource(fullPath, ResourceType.Markdown, location);
					break;
				case ".gen":
					page = new PageResource(fullPath, ResourceType.Generator, location);
					break;
				case ".rtf":
					page = new PageResource(fullPath, ResourceType.RichText, location);
					break;
				case ".html":
				case ".nomnoml":
					page = new PageResource(fullPath, ResourceType.Html, location);
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
			PageResources toAppend = new();
			foreach ((_, PageResource page) in pages)
			{
				foreach ((string key, PageResource value) in ProcessGenerator(site, pages, page))
					toAppend.Add(key, value);
			}

			foreach (KeyValuePair<string, PageResource> pair in toAppend)
				pages.Add(pair.Key, pair.Value);
		}
	}
	
	private static readonly Regex _generatorLinkRegex = new(@"\[(.+?)\]\(([\w%/]+?)\.md\)", RegexOptions.Compiled);

	public static IEnumerable<(string key, PageResource value)> ProcessGenerator(Site site, PageResources? allResources, PageResource page)
	{
		// Generated markdown
		if (page.FullPath.EndsWith("_sidebar.md.gen"))
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
			 *  - [Editor assembly definitions](Assembly%20Definitions.md)
			 *- [General advice](General%20Advice.md)
			 *- [Project reimport](Project%20Reimport.md)
			 */
			// E:\Projects\help.vertx.xyz\Troubleshooter\Assets\Site\Programming\Scripts\Loading\Script Loading_sidebar.md.gen
			
			string markdown = page.MarkdownText!;
			MatchCollection matches = _generatorLinkRegex.Matches(markdown);
			string directory = Path.GetDirectoryName(page.FullPath)!;
			for (var i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];
				string relativeLink = match.Groups[2].Value;
				string path = $"{Path.Combine(directory, relativeLink)}_sidebar.md";
				
				// Replace the link to *this* page with a bolded version
				string markdownText = markdown.Replace($"[{match.Groups[1].Value}]({relativeLink}.md)", $"**{match.Groups[1].Value}**");
				// Edit links to be local to the destination if required.
				if (relativeLink.Contains('/'))
				{
					string localDirectory = Path.GetDirectoryName(path)!;
					for (int j = 0; j < matches.Count; j++)
					{
						// Do not edit *this*, as it has been bolded already.
						if(i == j) continue;
						string originalPath = $"{Path.Combine(directory, matches[j].Groups[2].Value)}.md";
						string relativePath = Path.GetRelativePath(localDirectory, originalPath).Replace('\\', '/');
						markdownText = markdownText.Replace($"({matches[j].Groups[2].Value}.md)", $"({relativePath})");
					}
				}

				PageResource resource = new(path, ResourceType.Markdown, page.Location)
				{
					MarkdownText = markdownText
				};
				yield return (path, resource);
			}
		}
	}
}