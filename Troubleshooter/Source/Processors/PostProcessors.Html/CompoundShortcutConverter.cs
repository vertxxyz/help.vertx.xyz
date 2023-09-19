using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Converts kbd tag content to separated tags instead of using the + separator.
/// </summary>
[UsedImplicitly]
public sealed partial class CompoundShortcutConverter: IHtmlPostProcessor
{
	[GeneratedRegex(@"<kbd>(.+?\+.+?)</kbd>", RegexOptions.Compiled)]
	private static partial Regex GetCompoundKbdRegex();

	private static readonly Regex s_CompoundKbdRegex = GetCompoundKbdRegex();

	public string Process(string html, string fullPath)
	{
		return StringUtility.ReplaceMatch(html, s_CompoundKbdRegex, (match, builder) =>
		{
			string[] strings = match.Split("+");
			foreach (string s in strings)
			{
				builder.Append("<kbd>");
				builder.Append(s);
				builder.Append("</kbd>");
			}
		}, 1);
	}
}