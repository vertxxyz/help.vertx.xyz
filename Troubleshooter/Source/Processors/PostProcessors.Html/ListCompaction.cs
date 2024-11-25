using System;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// When a span with the tag "collapse" is found as a list element, it's assumed that the next tag should be in that list,
/// and that the list continues afterwards.
/// This logic will collapse the lists together and add the tag in between them into the list at that point.
/// </summary>
[UsedImplicitly]
public sealed partial class ListCompaction : IHtmlPostProcessor
{
	[GeneratedRegex("<li><span class=\"collapse\">collapse</span></li>\n</(ul|ol)>")]
	private static partial Regex EmptyListRegex { get; }

	public string Process(string html, string fullPath)
	{
		MatchCollection matches = EmptyListRegex.Matches(html);
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

	[GeneratedRegex("<([\\w]+) *[\\w =\"-.]*>")]
	private static partial Regex SimplifiedTagRegex { get; }

	private static int GetClosingTagEnd(string remaining)
	{
		Match firstTag = SimplifiedTagRegex.Match(remaining);
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
