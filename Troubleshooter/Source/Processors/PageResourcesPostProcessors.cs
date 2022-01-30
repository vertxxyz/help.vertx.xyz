using System;
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
			.Select(t => (IPageResourcesPostProcessor) Activator.CreateInstance(t)!).ToArray();

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
public class MainResourceProducer : IPageResourcesPostProcessor
{
	private readonly Regex firstLineRegex = new(@"^ *(.+)", RegexOptions.Compiled);
	private readonly Regex titleRegex = new(@"(?<!#)##(?!#)", RegexOptions.Compiled);
		
	public void Process(PageResources dictionary, Site site)
	{
		(string fullPath, PageResource main) = dictionary.First(pageResource => Path.GetFileName(pageResource.Key) == "Main.md");
		string directory = Path.GetDirectoryName(fullPath)!;
		string allText = File.ReadAllText(main.FullPath);
		string[] pages = titleRegex.Split(allText);
		foreach (string page in pages.Skip(1))
		{
			string title = firstLineRegex.Match(page).Groups[1].Value.Trim();
			StringBuilder stringBuilder = new StringBuilder(page);
			stringBuilder.Replace("##", "#");
			stringBuilder.Replace("###", "##");
			stringBuilder.Replace("####", "###");
			stringBuilder.Insert(0, "#");
			string destination = Path.Combine(directory, $"{title}.md");
			var newPage = new PageResource(destination, ResourceType.Markdown, ResourceLocation.Site);
			newPage.ProcessMarkdown(stringBuilder.ToString(), site, dictionary);
			dictionary.Add(destination, newPage);
		}
	}
}