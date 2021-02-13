using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RtfPipe;

namespace Troubleshooter
{
	public static class RtfUtility
	{
		public static string RtfToHtml(string rtf)
		{
			string html = Rtf.ToHtml(rtf);
			//Get closing index of font div
			int closing = IndexOfClosingChar(html, 0, '<', '>');
			html = html.Substring(closing + 1, html.Length - 6 - (closing + 1)); //6 is "</div>".Length
			html = Regex.Replace(html, @"font-size:\d+pt;", string.Empty);
			html = Regex.Replace(html, @"margin:\d+;", string.Empty);
			html = html.Replace(@"<p style=""", @"<span style=""");
			html = html.Replace("</p>", "</span>");
			ReplaceTabsHack();
			ReplaceHighlightHack();
			ReplaceUnusedHack();

			html = string.Concat("<div class=\"editor-colors\"><pre>", html, "</pre></div>");

			return html;

			void ReplaceTabsHack()
			{
				MatchCollection matchCollection = Regex.Matches(html, @"<span style=""display:inline-block;width:(\d+)px""></span>");

				if (matchCollection.Count == 0)
				{
					return;
				}

				StringBuilder stringBuilder = new StringBuilder(html.Length);
				int endIndex = 0;
				for (int i = 0; i < matchCollection.Count; i++)
				{
					Match match = matchCollection[i];
					stringBuilder.Append(html.Substring(endIndex, match.Index - endIndex));
					stringBuilder.Append("<span>");
					stringBuilder.Append(' ', (int) Math.Round(int.Parse(match.Groups[1].Value) * 0.083f));
					stringBuilder.Append("</span>");
					endIndex = match.Index + match.Length;
				}

				stringBuilder.Append(html[endIndex..]);

				html = stringBuilder.ToString();
			}

			void ReplaceHighlightHack() => html = html.Replace("background:#133F2F;", string.Empty);

			void ReplaceUnusedHack() => html = html.Replace("color:#787878;", "color:#BDBDBD;");
		}

		private static int IndexOfClosingChar(string expression, int index, char openChar, char closeChar)
		{
			int i;

			// If index given is invalid and is not an opening bracket.  
			if (expression[index] != openChar)
				throw new ArgumentOutOfRangeException($"Input index {index} into expression \"{expression}\" did not contain opening char {openChar}.");

			// Stack to store opening brackets.  
			Stack<int> st = new Stack<int>();

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
	}
}