using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Generates sub pages from Main.md that have content from each of the subheaders.
/// </summary>
[UsedImplicitly]
public partial class MainResourceProducer : IPageResourcesPostProcessor
{
	private static readonly Regex s_firstLineRegex = GetFirstLineRegex();
	private static readonly Regex s_titleRegex = GetTitleRegex();

	// Note that this processor is a bit of a hack, links on pages are hackily replaced with root links. Presume that all main pages are at the root.
	// If this changes then the script will need to be altered.
	private static readonly ImmutableDictionary<string, string?> s_mainPages = new Dictionary<string, string?>
	{
		{ "Index.md", null },
		{ "DOTS.md", "DOTS" }
	}.ToImmutableDictionary();

	public void Process(PageResourcesLookup dictionary, Arguments arguments, Site site)
	{
		KeyValuePair<string, PageResource>[] resources = dictionary.ToArray();
		foreach ((string fullPath, PageResource main) in resources)
		{
			// Only operate on main pages.
			if (!s_mainPages.TryGetValue(Path.GetFileName(fullPath), out string? destinationPath))
				continue;

			if ((main.Flags & ResourceFlags.Symlink) != 0) throw new BuildException("Detected main page was a symlink");

			main.MarkAsIndexPage();

			string directory = Path.GetDirectoryName(fullPath)!;
			if (!string.IsNullOrEmpty(destinationPath))
				directory = Path.Combine(directory, destinationPath);

			string allText = File.ReadAllText(main.FullPath);
			string[] pages = s_titleRegex.Split(allText);
			// Iterate over all the pages in this main page (pages are the content between the titles we used to split)
			foreach (string page in pages.Skip(1))
			{
				string title = s_firstLineRegex.Match(page).Groups[1].Value.Trim();
				StringBuilder stringBuilder = new(page);
				// Reduce the headers by moving them up a level
				stringBuilder.Replace("##", "#");
				stringBuilder.Replace("###", "##");
				stringBuilder.Replace("####", "###");
				// The title regex was used to split, we need to give the title back its header tag.
				stringBuilder.Insert(0, "#");

				// Change relative links to root-level links.
				int start = 0;
				while (true)
				{
					int index = stringBuilder.NextIndexOf("](", start);
					if (index == -1)
						break;
					start = index + 3;
					if (stringBuilder[index + 1] == '/')
						continue;
					stringBuilder.Insert(index + 2, '/');
				}

				string destination = Path.Combine(directory, title);

				if (!Directory.Exists(destination))
					continue;

				destination += ".md";
				var newPage = new PageResource(destination, ResourceType.Markdown, ResourceLocation.Site, null, arguments.Path, site);
				newPage.MarkAsIndexPage();
				newPage.ProcessMarkdown(stringBuilder.ToString(), site, dictionary);
				dictionary.Add(destination, newPage);
			}
		}
	}

	[GeneratedRegex("(?<!#)##(?!#)")]
	private static partial Regex GetTitleRegex();

	[GeneratedRegex("^ *(.+)")]
	private static partial Regex GetFirstLineRegex();
}
