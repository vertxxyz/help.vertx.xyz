using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Troubleshooter;

public static class PageUtility
{
	public static HashSet<string> GetAllSymlinkedFiles(string rootDirectory)
	{
		// Collect nested files.
		HashSet<string> files = CollectNestedSymlinkedFiles(CollectSymlinkedDirectories(rootDirectory)).Keys.ToHashSet();

		// Collect raw symlinks.
		foreach (string path in Directory.EnumerateFiles(rootDirectory, "*", SearchOption.AllDirectories))
		{
			if (new FileInfo(path).LinkTarget == null)
				continue;

			files.Add(path);
		}

		return files;
	}

	public static Dictionary<string, string> CollectNestedSymlinkedFiles(Dictionary<string, string> directorySymlinksFromTo)
	{
		Dictionary<string, string> fileSymlinksFromTo = new();
		foreach ((string fromDirectory, string toDirectory) in directorySymlinksFromTo)
		{
			// All these files are nested under symlinks
			foreach (string path in Directory.EnumerateFiles(fromDirectory, "*", SearchOption.AllDirectories))
				fileSymlinksFromTo.Add(path, Path.GetFullPath(Path.GetRelativePath(fromDirectory, path), toDirectory));
		}

		return fileSymlinksFromTo;
	}

	public static Dictionary<string, string> CollectSymlinkedDirectories(string rootDirectory)
	{
		Dictionary<string, string> directorySymlinksFromTo = new();
		foreach (string path in Directory.EnumerateDirectories(rootDirectory, "*", SearchOption.AllDirectories))
		{
			var directoryInfo = new DirectoryInfo(path);
			string? linkTarget = directoryInfo.LinkTarget;
			if (linkTarget == null)
				continue;

			// Is a symlink, add it to the list.
			directorySymlinksFromTo.Add(path, Path.GetFullPath(linkTarget, Path.GetDirectoryName(path)!));
		}

		return directorySymlinksFromTo;
	}

	/// <summary>
	/// Parse markdown text looking for page links
	/// </summary>
	/// <param name="markdownText">The markdown</param>
	/// <param name="path">The file path</param>
	/// <param name="contentRoot">The root of the site's content</param>
	/// <returns>The full path (without anchor links)</returns>
	public static IEnumerable<(string fullPath, Group group)> GetLinkFullPathsFromMarkdownText(string markdownText, string path, string contentRoot)
	{
		string directory = Path.GetDirectoryName(path) ?? string.Empty;
		MatchCollection matches = CommonRegex.InternalLinks.Matches(markdownText);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[2];
			string match = group.Value.ToWorkingPath().ToUnTokenized();
			if (match.Contains('#'))
				match = match[..match.IndexOf('#')];
			string fullPath = match.StartsWith('\\') ? Path.GetFullPath(match[1..], contentRoot) : Path.GetFullPath(Path.Combine(directory, match));
			yield return (fullPath, group);
		}
	}

	/// <summary>
	/// Converts a link's path to a full path. <see cref="contentRoot"/> is only used for paths starting with a slash. <see cref="directory"/> is the root of the link.
	/// </summary>
	public static string GetFullPathFromTokenizedLink(string directory, string linkPath, string contentRoot)
	{
		string unTokenizedLinkPath = linkPath.ToWorkingPath().ToUnTokenized();
		string fullPath = unTokenizedLinkPath.StartsWith('\\') ? Path.GetFullPath(unTokenizedLinkPath[1..], contentRoot) : Path.GetFullPath(Path.Combine(directory, unTokenizedLinkPath));
		return fullPath;
	}

	public static string GetFullPathFromLocalEmbed(string embedPath, Site site)
		=> Path.GetFullPath(Path.Combine(site.EmbedsDirectory, embedPath));

	/// <summary>
	/// Parse markdown text looking for external links
	/// </summary>
	/// <param name="text">The markdown</param>
	/// <returns></returns>
	public static IEnumerable<(string url, Group group)> GetExternalLinkUrlsFromMarkdownText(string text)
	{
		// - I have no need for this other than debugging at the moment, so the fact that it captures brackets following links also does not currently matter.
		MatchCollection matches = CommonRegex.ExternalLink.Matches(text);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[1];
			string url = group.Value;
			yield return (url, group);
		}
	}

	public static IEnumerable<(string localPath, Group group)> GetEmbedsAsLocalPathsFromMarkdownText(string text)
	{
		MatchCollection matches = CommonRegex.Embeds.Matches(text);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[1];
			yield return (group.Value.ToWorkingPath().ToUnTokenized(), group);
		}
	}

	public static IEnumerable<(string localPath, Group group)> GetImagesAsLocalPathsFromMarkdownText(string text, bool finalisePath = true)
	{
		MatchCollection matches = CommonRegex.LocalImages.Matches(text);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[1];
			yield return (finalisePath ? group.Value.ToFinalisedWorkingPath() : group.Value, group);
		}
	}
}
