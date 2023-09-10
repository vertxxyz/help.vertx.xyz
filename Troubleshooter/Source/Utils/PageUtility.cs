using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Troubleshooter;

public static partial class PageUtility
{
	private static readonly Regex pathRegex = GetPathRegex();
	private static readonly Regex linkRegex = GetLinkRegex();
	private static readonly Regex embedsRegex = GetEmbedsRegex();
	private static readonly Regex localImagesRegex = GetLocalImagesRegex();

	/// <summary>
	/// Parse markdown text looking for page links
	/// </summary>
	/// <param name="text">The markdown</param>
	/// <param name="path">The file path</param>
	/// <param name="root">The root of the site's content</param>
	/// <returns></returns>
	public static IEnumerable<(string fullPath, Group group)> LinksAsFullPaths(string text, string path, string root)
	{
		string directory = Path.GetDirectoryName(path) ?? string.Empty;
		MatchCollection matches = pathRegex.Matches(text);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[1];
			string match = group.Value.ToConsistentPath().ToUnTokenized();
			string fullPath = match.StartsWith('/') ? Path.GetFullPath(match[1..], root) : Path.GetFullPath(Path.Combine(directory, match));
			yield return (fullPath, group);
		}
	}

	/// <summary>
	/// Parse markdown text looking for external links
	/// </summary>
	/// <param name="text">The markdown</param>
	/// <returns></returns>
	public static IEnumerable<(string url, Group group)> ExternalLinkUrls(string text)
	{
		//TODO someone who is better at regex than me can feel free to make this nicer.
		// - I have no need for this other than debugging at the moment, so the fact that it captures brackets following links also does not currently matter.
		MatchCollection matches = linkRegex.Matches(text);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[1];
			string url = group.Value;
			yield return (url, group);
		}
	}

	public static IEnumerable<(string localPath, Group group)> EmbedsAsLocalEmbedPaths(string text)
	{
		MatchCollection matches = embedsRegex.Matches(text);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[1];
			yield return (group.Value.ToConsistentPath().ToUnTokenized(), group);
		}
	}

	public static IEnumerable<(string localPath, Group group)> LocalImagesAsRootPaths(string text, bool finalisePath = true)
	{
		MatchCollection matches = localImagesRegex.Matches(text);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[1];
			yield return (finalisePath ? group.Value.FinalisePath() : group.Value, group);
		}
	}

	public static string LocalEmbedToFullPath(string embedPath, Site site)
		=> Path.GetFullPath(Path.Combine(site.EmbedsDirectory, embedPath));

	[GeneratedRegex(@"(?<!!)\[.+?\]\(([\w /%.]+)\)", RegexOptions.Compiled)]
	private static partial Regex GetPathRegex();

	[GeneratedRegex(@"]\((https?://[\w/%#?.@_+~=&()]+)\)", RegexOptions.Compiled)]
	private static partial Regex GetLinkRegex();

	[GeneratedRegex(@"<<([A-Za-z0-9\-/ ]+?\.[a-zA-z]+?)>>", RegexOptions.Compiled)]
	private static partial Regex GetEmbedsRegex();

	[GeneratedRegex("!\\[[^\\]]*\\]\\((?!http)(.*?)\\s*(\".*[^\"]\")?\\s*\\)", RegexOptions.Compiled)]
	private static partial Regex GetLocalImagesRegex();
}