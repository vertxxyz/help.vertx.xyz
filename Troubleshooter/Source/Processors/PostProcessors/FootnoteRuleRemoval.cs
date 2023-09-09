using System;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Removes the horizontal rule in the footnotes block.
/// </summary>
[UsedImplicitly]
public sealed class FootnoteRuleRemoval : IHtmlPostProcessor
{
	public string Process(string html, string fullPath)
	{
		int footnoteIndex = html.IndexOf("<div class=\"footnotes\">", StringComparison.Ordinal);
		if (footnoteIndex < 0)
			return html;
		const int length = 23; // "<div class=\"footnotes\">".Length;
		return html.Substring(footnoteIndex + length + 1, 6) != "<hr />"
			? html
			: string.Concat(html[..(footnoteIndex + length)], html[(footnoteIndex + length + 7)..]);
	}
}