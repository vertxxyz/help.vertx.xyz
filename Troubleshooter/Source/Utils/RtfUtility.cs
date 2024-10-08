using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RtfPipe;

namespace Troubleshooter;

public static partial class RtfUtility
{
	private static readonly Regex s_fontSizeRegex = GetFontSizeRegex();
	private static readonly Regex s_tabsRegex = GetTabsRegex();
	private static readonly Regex s_backgroundRegex = GetBackgroundRegex();
	private static readonly Regex s_marginsRegex = GetMarginsRegex();
	private static readonly Regex s_colorRegex = GetColorRegex();

	public static string RtfToHtml(string rtf)
	{
		string html = Rtf.ToHtml(rtf);
		// Get closing index of font div
		int closing = IndexOfClosingChar(html, 0, '<', '>');
		html = html.Substring(closing + 1, html.Length - 6 - (closing + 1)); //6 is "</div>".Length
		html = s_fontSizeRegex.Replace(html, string.Empty);
		html = s_marginsRegex.Replace(html, string.Empty);
		html = s_backgroundRegex.Replace(html, string.Empty);
		// Replace all paragraphs with spans
		html = html.Replace(@"<p style=""", @"<span style=""");
		html = html.Replace("</p>", "</span>");
		ReplaceTabsHack();
		ReplaceUnusedHack();
		ReplaceColorsHack();
		RemoveEmptyStyleHack();
		FixRootLevelStyleHack();

		StringBuilder sb = HtmlUtility.AppendWithCodeBlockSetup(html);
		return sb.ToString();

		// Replace explicit width with spaces
		void ReplaceTabsHack()
		{
			html = StringUtility.ReplaceMatch(html, s_tabsRegex, (match, stringBuilder) =>
			{
				stringBuilder.Append("<span>");
				stringBuilder.Append(' ', (int)Math.Round(int.Parse(match) * 0.083f));
				stringBuilder.Append("</span>");
			}, 1);
		}

		void ReplaceUnusedHack() => html = html.Replace("color:#787878;", string.Empty);

		// Replace all the explicit colour styles with HTML instead
		void ReplaceColorsHack()
		{
			html = StringUtility.ReplaceMatch(html, s_colorRegex, (match, stringBuilder) =>
			{
				if (HtmlUtility.ColorToClassLookup.TryGetValue(match.Groups[1].Value, out string? className))
				{
					int startOfTag = stringBuilder.LastIndexOf('<') + 1;
					int nextSpace = stringBuilder.NextIndexOf(' ', startOfTag);
					stringBuilder.Insert(nextSpace, " class=\"");
					stringBuilder.Insert(nextSpace + 8, className);
					int index = nextSpace + 8 + className.Length;
					stringBuilder.Insert(index, "\"");
					if (stringBuilder[index + 1] != '>')
						stringBuilder.Insert(index + 1, ' ');
				}
				else
				{
					// - do nothing, just re-append the match
					stringBuilder.Append(match.Value);
				}
			});
		}

		// Remove empty style blocks
		void RemoveEmptyStyleHack() => html = html.Replace("""
		                                                    style=""
		                                                   """, string.Empty);

		// Anything that isn't explicitly styled should have a style
		void FixRootLevelStyleHack() => html = html.Replace("<span>", """<span class="token punctuation">""");
	}

	private static int IndexOfClosingChar(string expression, int index, char openChar, char closeChar)
	{
		int i;

		// If index given is invalid and is not an opening bracket.
		if (expression[index] != openChar)
			throw new ArgumentOutOfRangeException($"Input index {index} into expression \"{expression}\" did not contain opening char {openChar}.");

		// Stack to store opening brackets.
		Stack<int> st = new();

		// Traverse through string starting from given index.
		for (i = index; i < expression.Length; i++)
		{
			// If current character is an opening bracket push it in stack.
			if (expression[i] == openChar)
				st.Push(expression[i]);
			// If current character is 'closing', pop from stack. If stack is empty, then this is the required 'closing'.
			else if (expression[i] == closeChar)
			{
				st.Pop();
				if (st.Count == 0)
					return i;
			}
		}

		return -1;
	}

	[GeneratedRegex("font-size:\\d+pt;")]
	private static partial Regex GetFontSizeRegex();

	[GeneratedRegex("<span style=\"display:inline-block;width:(\\d+)px\"></span>")]
	private static partial Regex GetTabsRegex();

	[GeneratedRegex("background:#\\w{6};")]
	private static partial Regex GetBackgroundRegex();

	[GeneratedRegex("margin:\\d+;")]
	private static partial Regex GetMarginsRegex();

	[GeneratedRegex("color:(#\\w{6});")]
	private static partial Regex GetColorRegex();
}
