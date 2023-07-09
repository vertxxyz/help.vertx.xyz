using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Troubleshooter.Constants;

namespace Troubleshooter;

/// <summary>
/// Runs on HTML after it has been processed from markdown.
/// </summary>
public static class PageResourcesPostProcessors
{
	private static readonly IPageResourcesPostProcessor[] All =
		typeof(IPageResourcesPostProcessor).Assembly.GetTypes()
			.Where(t => typeof(IPageResourcesPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IPageResourcesPostProcessor)Activator.CreateInstance(t)!).ToArray();

	public static void Process(PageResources pageResources, Site site)
	{
		foreach (IPageResourcesPostProcessor postProcessor in All)
			postProcessor.Process(pageResources, site);
	}
}

/// <summary>
/// Generates sub pages from Main.md that have content from each of the subheaders.
/// </summary>
[UsedImplicitly]
public partial class MainResourceProducer : IPageResourcesPostProcessor
{
	private static readonly Regex s_FirstLineRegex = GetFirstLineRegex();
	private static readonly Regex s_TitleRegex = GetTitleRegex();
	private static readonly ImmutableDictionary<string, string?> s_MainPages = new Dictionary<string, string?>
	{
		{ "Main.md", null },
		{ "DOTS.md", "DOTS" }
	}.ToImmutableDictionary();

	public void Process(PageResources dictionary, Site site)
	{
		KeyValuePair<string,PageResource>[] resources = dictionary.ToArray();
		foreach ((string fullPath, PageResource main) in resources)
		{
			if (!s_MainPages.TryGetValue(Path.GetFileName(fullPath), out string? destinationPath))
				continue;

			string directory = Path.GetDirectoryName(fullPath)!;
			if (!string.IsNullOrEmpty(destinationPath))
				directory = Path.Combine(directory, destinationPath);
			
			string allText = File.ReadAllText(main.FullPath);
			string[] pages = s_TitleRegex.Split(allText);
			foreach (string page in pages.Skip(1))
			{
				string title = s_FirstLineRegex.Match(page).Groups[1].Value.Trim();
				StringBuilder stringBuilder = new(page);
				stringBuilder.Replace("##", "#");
				stringBuilder.Replace("###", "##");
				stringBuilder.Replace("####", "###");
				stringBuilder.Insert(0, "#");
				string destination = Path.Combine(directory, title);

				if (!Directory.Exists(destination))
					continue;
				
				destination += ".md";
				var newPage = new PageResource(destination, ResourceType.Markdown, ResourceLocation.Site);
				newPage.ProcessMarkdown(stringBuilder.ToString(), site, dictionary);
				dictionary.Add(destination, newPage);
			}
		}
	}

	[GeneratedRegex("(?<!#)##(?!#)", RegexOptions.Compiled)]
	private static partial Regex GetTitleRegex();

	[GeneratedRegex("^ *(.+)", RegexOptions.Compiled)]
	private static partial Regex GetFirstLineRegex();
}