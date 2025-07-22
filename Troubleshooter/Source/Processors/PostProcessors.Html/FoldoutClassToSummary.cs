using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public partial class FoldoutClassToSummary : IHtmlPostProcessor
{
	[GeneratedRegex("""
	                class="[^"]*?\bfoldout\b[^"]*?"
	                """
	)]
	private static partial Regex FoldoutRegex { get; }

	private const string StartDetailsTag = "<details class=\"from-foldout\">";
	private const string StartDetailsTagFigure = "<details class=\"from-foldout figure-foldout\">";
	private const string StartSummaryTags = "<summary>";
	private const string EndSummaryTags = "</summary>";
	private const string EndTags = "</details>";

	// Opening to closing tags
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
			html = ProcessSingleFoldout(html, match);
		}

		return html;
	}

	private string ProcessSingleFoldout(string html, Match match)
	{
		int start = html.LastIndexOf('<', match.Index);
		int tagEnd = html.IndexOf(' ', start);
		string tag = html[(start + 1)..tagEnd];
		string endTag = $"</{tag}>";
		int endSummaryIndex = html.IndexOf(endTag, match.Index, StringComparison.Ordinal);
		if (endSummaryIndex == -1)
		{
			throw new LogicException($"Terminating tag </{tag}> could not be found");
		}

		switch (tag)
		{
			case "figcaption":
				return ProcessFigureFoldout(html, start, endSummaryIndex + endTag.Length);
		}

		if (!_closingTags.TryGetValue(tag, out string[]? tags))
		{
			throw new NotImplementedException($"Tag {tag} is not a supported tag with \".foldout\", update this code.");
		}

		int detailsClosingIndex = html.Length - 1;

		foreach (string closingTag in tags)
		{
			int length = detailsClosingIndex - endSummaryIndex;
			int nextIndex = html.IndexOf(closingTag, endSummaryIndex, length, StringComparison.Ordinal);
			if (nextIndex == -1)
				continue;
			detailsClosingIndex = Math.Min(detailsClosingIndex, nextIndex);
		}

		// Use calculated info to insert details + summary blocks. Starting at the end and working backwards.
		html = html.Insert(detailsClosingIndex, EndTags);
		html = html.Insert(endSummaryIndex, EndSummaryTags);
		html = html.Insert(start, StartSummaryTags);
		html = html.Insert(start, StartDetailsTag);
		return html;
	}

	private static string ProcessFigureFoldout(string html, int startSummary, int endSummary)
	{
		// Stupidly Markdig creates an extra figcaption element when adding a class to a figure.
		// So, we remove that caption:
		html = html.Remove(startSummary, endSummary - startSummary);
		// Then we find the next occurence of the summary:
		startSummary = html.IndexOf("<figcaption>", startSummary, StringComparison.Ordinal) + "<figcaption>".Length;
		if (endSummary == -1)
		{
			throw new LogicException("Next <figcaption> could not be found");
		}

		endSummary = html.IndexOf("</figcaption>", startSummary, StringComparison.Ordinal);
		if (endSummary == -1)
		{
			throw new LogicException("Terminating tag </figcaption> could not be found");
		}

		string summary = html[startSummary..endSummary];
		int startRemove = startSummary - "<figcaption>".Length;
		int endRemove = endSummary  + "</figcaption>".Length;
		html = html.Remove(startRemove, endRemove - startRemove);

		const string detailsTag = "figure";
		int detailsStart = html.LastIndexOf(detailsTag, startRemove, StringComparison.Ordinal);
		detailsStart = html.LastIndexOf('<', detailsStart);
		int detailsEnd = html.IndexOf("</figure>", startRemove, StringComparison.Ordinal) + "</figure>".Length;

		// Use calculated info to insert details + summary blocks. Starting at the end and working backwards.
		html = html.Insert(detailsEnd, EndTags);
		html = html.Insert(detailsStart, EndSummaryTags);
		html = html.Insert(detailsStart, summary);
		html = html.Insert(detailsStart, StartSummaryTags);
		html = html.Insert(detailsStart, StartDetailsTagFigure);
		return html;
	}
}
