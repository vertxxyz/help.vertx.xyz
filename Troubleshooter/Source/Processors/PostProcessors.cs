using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using TwemojiSharp;

namespace Troubleshooter;

/// <summary>
/// Runs on HTML after it has been processed from markdown.
/// </summary>
public static class HtmlPostProcessors
{
	private static readonly IHtmlPostProcessor[] All =
		typeof(IHtmlPostProcessor).Assembly.GetTypes()
			.Where(t => typeof(IHtmlPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IHtmlPostProcessor)Activator.CreateInstance(t)!).OrderBy(p => p.Order).ToArray();

	public static string Process(string html, string fullPath) => All.Aggregate(html, (current, processor) => processor.Process(current, fullPath));
}

[UsedImplicitly]
public sealed class RelativeLinkConverter : IHtmlPostProcessor
{
	private readonly Regex regex = new(@"(?<=<a )href=""([^""]+\.md)""", RegexOptions.Compiled);

	public string Process(string html, string fullPath) =>
		StringUtility.ReplaceMatch(html, regex, (group, stringBuilder) =>
		{
			var insert = group.Replace("&amp;", "and");
			insert = insert.Replace("&", "and");
			insert = StringUtility.ToLowerSnakeCase(insert);
			stringBuilder.Append($"onclick=\"loadPage(\'{insert}\')\"");
		}, 1);
}

[UsedImplicitly]
public sealed class ExternalLinkConverter : IHtmlPostProcessor
{
	private readonly Regex regex = new(@"(?<=<a )href=""https?:\/\/[\.\w\/\-%#?=@_]+""", RegexOptions.Compiled);

	public int Order => 1;

	public string Process(string html, string fullPath) =>
		StringUtility.ReplaceMatch(html, regex, (group, stringBuilder) =>
		{
			stringBuilder.Append(@"class=""link--external"" ");
			stringBuilder.Append(group);
		});
}

[UsedImplicitly]
public sealed class BooleanTableConverter : IHtmlPostProcessor
{
	public string Process(string html, string fullPath)
	{
		html = html.Replace("<td>Y</td>", "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableYes\"></td>");
		return html.Replace("<td>N</td>", "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableNo\"></td>");
	}
}

[UsedImplicitly]
public sealed class SliderConverter : IHtmlPostProcessor
{
	private readonly Regex regex = new("<div.* class=\".*?slider\"></div>", RegexOptions.Compiled);

	public string Process(string html, string fullPath)
	{
		return StringUtility.ReplaceMatch(html, regex, (group, stringBuilder) =>
		{
			stringBuilder.Append(group[..^6]);
			{
				stringBuilder.Append("<div class=\"slider_container\">");
				stringBuilder.Append("<div class=\"slider_left_gutter\"></div>");
				stringBuilder.Append("<div class=\"slider_right_gutter\"></div>");
				stringBuilder.Append("<div class=\"slider_knob_container\">");
				stringBuilder.Append("<div class=\"slider_knob\"></div>");
				stringBuilder.Append("</div>");
				stringBuilder.Append("</div>");
				//<div class="slider_container">
				//	<div class="slider_left_gutter"></div>
				//	<div class="slider_right_gutter"></div>
				//	<div class="slider_knob_container">
				//		<div class="slider_knob"></div>
				//	</div>
				//</div>
			}
			stringBuilder.Append("</div>");
		}, 0);
	}
}

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

/// <summary>
/// When a span with the tag "collapse" is found as a list element, it's assumed that the next tag should be in that list,
/// and that the list continues afterwards.
/// This logic will collapse the lists together and add the tag in between them into the list at that point.
/// </summary>
[UsedImplicitly]
public sealed class ListCompaction : IHtmlPostProcessor
{
	private static readonly Regex emptyListRegex = new("<li><span class=\"collapse\">collapse</span></li>\n</(ul|ol)>", RegexOptions.Compiled);
	
	public string Process(string html, string fullPath)
	{
		MatchCollection matches = emptyListRegex.Matches(html);
		if (matches.Count == 0) return html;
		int last = 0;
		StringBuilder builder = new();
		for (int i = 0; i < matches.Count; i++)
		{
			Match match = matches[i];
			builder.Append(html[last..match.Index]);
			
			string remaining = html[(match.Index + match.Length)..];
			//builder.AppendLine("<ul>");
			builder.Append("<li>");
			int end = GetClosingTagEnd(remaining);
			builder.Append(remaining[..(end + 1)]);
			builder.AppendLine("</li>");

			ReadOnlySpan<char> after = remaining.AsSpan()[(end + 1)..];
			int index = end + 1 + after.IndexOf($"<{match.Groups[1].Value}>");
			last = match.Index + match.Length + index + 5;
		}

		builder.Append(html[last..]);
		return builder.ToString();
	}
	
	private static readonly Regex simplifiedTag = new("<([\\w]+) *[\\w =\"-.]*>", RegexOptions.Compiled);

	private static int GetClosingTagEnd(string remaining)
	{
		Match firstTag = simplifiedTag.Match(remaining);
		string tagType = firstTag.Groups[1].Value;
		int depth = 0;
		string closing = $"</{tagType}>";
		string opening = $"<{tagType}";
		int start = firstTag.Index + firstTag.Length;
		ReadOnlySpan<char> r = remaining.AsSpan()[start..];
		while (true)
		{
			int nextClosing = r.IndexOf(closing, StringComparison.Ordinal);
			int nextOpening = r.IndexOf(opening, StringComparison.Ordinal);
			if (nextOpening != -1 && nextOpening < nextClosing)
			{
				depth++;
				int nextOpen = nextOpening + opening.Length;
				r = r[nextOpen..];
				start += nextOpen;
				continue;
			}

			if (depth-- == 0)
				return start + nextClosing + closing.Length;
			int nextClose = nextClosing + closing.Length;
			r = r[nextClose..];
			start += nextClose;
		}
	}
}

/// <summary>
/// Replaces all emoji with Twemoji
/// </summary>
[UsedImplicitly]
public class TwemojiReplacement : IHtmlPostProcessor
{
	private readonly TwemojiLib twemoji = new();
	
	public string Process(string html, string fullPath) => twemoji.Parse(html);
}