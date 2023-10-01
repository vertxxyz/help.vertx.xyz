using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Troubleshooter;

public static class PageUtility
{
	public static Dictionary<string, string> CollectSymlinkedFilesLookup(string rootDirectory)
	{
		Dictionary<string, string> fileSymlinksFromTo = new();

		// For every symlink directory, iterate the file entries.
		foreach (string directory in Directory.EnumerateDirectories(rootDirectory, "*", SearchOption.AllDirectories))
		{
			string? linkTarget = new DirectoryInfo(directory).LinkTarget;
			if (linkTarget == null)
				continue;

			// Iterate to destination
			string path = directory;
			string? testLinkTarget = linkTarget;
			do
			{
				linkTarget = Path.GetFullPath(testLinkTarget, Path.GetDirectoryName(path)!);
				testLinkTarget = new FileInfo(linkTarget).LinkTarget;
				path = linkTarget;
			} while (testLinkTarget != null);

			foreach (string file in Directory.EnumerateFiles(linkTarget, "*", SearchOption.AllDirectories))
			{
				(string from, string to) = AppendEntryFileEntryIfRequired(file) ?? (file, file);
				from = Path.GetFullPath(Path.GetRelativePath(linkTarget, from), directory);
				if (fileSymlinksFromTo.TryAdd(from, to))
					continue;

				if (fileSymlinksFromTo[from] != to)
					throw new BuildException($"{from}->{to} does not match previously added ->{fileSymlinksFromTo[from]}");
			}
		}

		// Get straight-forward file-based symlinks.
		foreach (string file in Directory.EnumerateFiles(rootDirectory, "*", SearchOption.AllDirectories))
		{
			if (fileSymlinksFromTo.ContainsKey(file)) continue;

			(string from, string to)? result = AppendEntryFileEntryIfRequired(file);
			if (result.HasValue)
				fileSymlinksFromTo.Add(result.Value.from, result.Value.to);
		}

		return fileSymlinksFromTo;

		(string from, string to)? AppendEntryFileEntryIfRequired(string file)
		{
			string? linkTarget = new FileInfo(file).LinkTarget;
			if (linkTarget == null)
				return null;

			// Iterate to destination
			string path = file;
			string? testLinkTarget = linkTarget;
			do
			{
				linkTarget = Path.GetFullPath(testLinkTarget, Path.GetDirectoryName(path)!);
				testLinkTarget = new FileInfo(linkTarget).LinkTarget;
				path = linkTarget;
			} while (testLinkTarget != null);

			return (file, linkTarget);
		}
	}

	public static HashSet<string> GetAllSymlinkedFiles(string rootDirectory) => CollectSymlinkedFilesLookup(rootDirectory).Keys.ToHashSet();

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
			int hashIndex = match.IndexOf('#');
			switch (hashIndex)
			{
				case 0:
					continue;
				case > 0:
					match = match[..hashIndex];
					break;
			}

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
