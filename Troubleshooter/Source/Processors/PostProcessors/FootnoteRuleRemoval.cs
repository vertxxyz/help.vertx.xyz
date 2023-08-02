using System;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public sealed class FootnoteRuleRemoval : IHtmlPostProcessor
{
	public string Process(string html, string fullPath)
	{
		int footnoteIndex = html.IndexOf("<div class=\"footnotes\">", StringComparison.Ordinal);
		if (footnoteIndex < 0)
			return html;
		const int length = 23; // "<div class=\"footnotes\">".Length;
		if (html.Substring(footnoteIndex + length + 1, 6) != "<hr />")
			return html;
		return string.Concat(html[..(footnoteIndex + length)], html[(footnoteIndex + length + 7)..]);
	}
}