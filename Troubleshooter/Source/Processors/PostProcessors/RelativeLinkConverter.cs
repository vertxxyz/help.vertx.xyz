
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public sealed partial class RelativeLinkConverter : IHtmlPostProcessor
{
	[GeneratedRegex("(?<=<a )href=\"([^\"]+\\.md)\"", RegexOptions.Compiled)]
	private static partial Regex GetRelativeLinkRegex();

	private static readonly Regex s_RelativeLinkRegex = GetRelativeLinkRegex();

	public string Process(string html, string fullPath) =>
		StringUtility.ReplaceMatch(html, s_RelativeLinkRegex, (group, stringBuilder) =>
		{
			var insert = group.Replace("&amp;", "and");
			insert = insert.Replace("&", "and");
			insert = StringUtility.ToLowerSnakeCase(insert);
			stringBuilder.Append($"onclick=\"loadPage(\'{insert}\')\"");
		}, 1);
}