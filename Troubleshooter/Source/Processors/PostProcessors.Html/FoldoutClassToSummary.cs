using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public partial class FoldoutClassToSummary : IHtmlPostProcessor
{
	[GeneratedRegex("""
	                class="[^"]?\bfoldout\b[^"]?"
	                """)]
	private static partial Regex FoldoutRegex { get; }

	private const string StartDetailsTag = "<details class=\"from-foldout\">";
	private const string StartTags = StartDetailsTag + "<summary>";
	private const string EndSummaryTags = "</summary>";
	private const string EndTags = "</details>";
	private readonly Dictionary<string, string[]> _closingTags = new()
	{
		{ "h1", [StartDetailsTag, "<hr", "<h1 "] },
		{ "h2", [StartDetailsTag, "<hr", "<h2 ", "<h1 "] },
		{ "h3", [StartDetailsTag, "<hr", "<h3 ", "<h2 ", "<h1 "] },
		{ "h4", [StartDetailsTag, "<hr", "<h4 ", "<h3 ", "<h2 ", "<h1 "] },
		{ "h5", [StartDetailsTag, "<hr", "<h5 ", "<h4 ", "<h3 ", "<h2 ", "<h1 "] },
	};

	public string Process(string html, string fullPath)
	{
		MatchCollection matches = FoldoutRegex.Matches(html);
		if (matches.Count == 0)
			return html;

		for (int i = matches.Count - 1; i >= 0; i--)
		{
			Match match = matches[i];
			int start = html.LastIndexOf('<', match.Index);
			int tagEnd = html.IndexOf(' ', start);
			string tag = html[(start + 1)..tagEnd];
			int endSummaryIndex = html.IndexOf($"</{tag}>", match.Index, StringComparison.Ordinal);
			if (endSummaryIndex == -1)
				throw new LogicException($"Terminating tag </{tag}> could not be found");
			if (!_closingTags.TryGetValue(tag, out string[]? closingTags))
				throw new NotImplementedException($"Tag {tag} is not a supported tag with \".foldout\", update this code.");
			int closestClosingIndex = html.Length - 1;
			foreach (string closingTag in closingTags)
			{
				int length = closestClosingIndex - endSummaryIndex;
				int nextIndex = html.IndexOf(closingTag, endSummaryIndex, length, StringComparison.Ordinal);
				if (nextIndex == -1)
					continue;
				closestClosingIndex = Math.Min(closestClosingIndex, nextIndex);
			}

			// Use calculated info to insert details + summary blocks. Starting at the end and working backwards.
			html = html.Insert(closestClosingIndex, EndTags);
			html = html.Insert(endSummaryIndex, EndSummaryTags);
			html = html.Insert(start, StartTags);
		}

		return html;
	}
}
