
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Converts relative links to call onclick and load the link via Javascript.
/// </summary>
[UsedImplicitly]
public sealed partial class RelativeLinkConverter : IHtmlPostProcessor
{
	[GeneratedRegex("(?<=<a )href=\"([^\"]+\\.md)\"", RegexOptions.Compiled)]
	private static partial Regex GetRelativeLinkRegex();

	private static readonly Regex s_relativeLinkRegex = GetRelativeLinkRegex();

	public string Process(string html, string fullPath) =>
		StringUtility.ReplaceMatch(html, s_relativeLinkRegex, (group, stringBuilder) =>
		{
			var insert = group.Replace("&amp;", "and");
			insert = insert.Replace("&", "and");
			insert = StringUtility.ToLowerSnakeCase(insert);
			stringBuilder.Append($"onclick=\"loadPage(\'{insert}\')\"");
		}, 1);
}
