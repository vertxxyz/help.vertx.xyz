using System.IO;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Converts relative links to call onclick and load the link via Javascript.
/// </summary>
[UsedImplicitly]
public sealed partial class RelativeLinkConverter : IHtmlPostProcessor
{
	[GeneratedRegex("""
	                (?<=<a )href="([^"]+?\.md(?:#[^"]+?)?)"
	                """)]
	private static partial Regex GetRelativeLinkRegex();

	private static readonly Regex s_relativeLinkRegex = GetRelativeLinkRegex();

	public string Process(string html, string fullPath) =>
		StringUtility.ReplaceMatch(html, s_relativeLinkRegex, (group, stringBuilder) =>
		{
			string insert = group.Replace("&amp;", "and");
			insert = insert.Replace("&", "and");
			insert = StringUtility.ToLowerSnakeCase(insert);
			string insertHref = Path.HasExtension(insert) ? Path.ChangeExtension(insert, ".html") : insert;
			stringBuilder.Append($"href=\"{insertHref}\" onclick=\"loadPage(\'{insert}\');return false;\" class=\"link--internal\"");
		}, 1);
}
