using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AdvancedStringBuilder;

namespace Troubleshooter
{
	public static class HtmlUtility
	{
		public static string Parse(string text)
		{
			if (text.Contains("code_block_"))
			{
				// Text is copied using "Copy With Style" plugin and needs to be processed as code.
				return ProcessAsCode(text);
			}
			
			return text;
		}
		
		private static readonly Regex whitespaceRegex = new(@"(^ +)|( +$)", RegexOptions.Compiled | RegexOptions.Multiline); // Captures whitespace at the start or end of the line
		private static readonly Regex styleRegex = new(@"\.(_style_\d+){color:(#\w{6});", RegexOptions.Compiled); // Captures style and associated color information
		private static readonly Regex styleToRemoveRegex = new(@"\.(_style_\d+){(?!color:)", RegexOptions.Compiled); // Captures style information for unmatched styles
		
		public static readonly Dictionary<string, string> ColorToClassLookup = new()
		{
			{ "#C191FF", "token attr-value" },
			{ "#6C95EB", "token keyword" },
			{ "#E1BFFF", "token struct" },
			{ "#39CC8F", "token function" },
			{ "#85C46C", "token commentColorOnly" },
			{ "#66C3CC", "token member" },
			{ "#C9A26D", "token string" },
			{ "#ED94C0", "token number" },
			{ "#D0D0D0", "token body" },
			{ "#BDBDBD", "token punctuation" }
		};

		private static string ProcessAsCode(string text)
		{
			int styleStart = text.IndexOf("<style>", StringComparison.Ordinal);
			int styleEnd = text.IndexOf("</style>", StringComparison.Ordinal);
			string styleContent = text[(styleStart + 7)..(styleEnd - 1)];
			
			// Collect inline style information
			MatchCollection styles = styleRegex.Matches(styleContent);
			Dictionary<string, string> styleToReplacement = new Dictionary<string, string>();
			foreach (Match match in styles)
			{
				string color = match.Groups[2].Value.ToUpperInvariant();
				string style = match.Groups[1].Value;
				if (!ColorToClassLookup.TryGetValue(color, out var replacementStyle))
					continue;

				styleToReplacement.Add(style, replacementStyle);
			}

			List<string> stylesToRemove = new List<string>();
			styles = styleToRemoveRegex.Matches(styleContent);
			foreach (Match match in styles)
			{
				string style = match.Groups[1].Value;
				stylesToRemove.Add(style);
			}

			string content = text[text.IndexOf('<', styleEnd + 8)..(text.LastIndexOf('>', text.Length - 6) + 1)];
			StringBuilder stringBuilder = new StringBuilder(content);
			// Remove unnecessary styles
			stringBuilder.Replace("_style_line", string.Empty);
			stringBuilder.Replace("_style_span ", string.Empty);
			// Remove empty classes
			stringBuilder.Replace(" class=\"\"", string.Empty);
			stringBuilder.Replace(" class=\" \"", string.Empty);
			// Remove paragraphs
			stringBuilder.Replace("<p>", string.Empty);
			stringBuilder.Replace("</p>", string.Empty);
			// Remove divs
			stringBuilder.Replace("<div class=\"_style_default\">", string.Empty);
			stringBuilder.Replace("/<div", string.Empty);
			stringBuilder.TrimStart();
			stringBuilder.TrimEnd();
			// Replace inline styles with final styles.
			foreach ((string style, string replacement) in styleToReplacement)
				stringBuilder.Replace(style, replacement);
			foreach (string toRemove in stylesToRemove)
				stringBuilder.Replace(toRemove, string.Empty);

			string html = stringBuilder.ToString();
			html = whitespaceRegex.Replace(html, string.Empty);
			html = html.Replace($"<span></span>{Environment.NewLine}", Environment.NewLine);
			html = html.Replace($"{Environment.NewLine}{Environment.NewLine}", "<br>");
			// Cleanup empty spaces in classes
			html = html.Replace("\" ", "\"");
			html = html.Replace(" \"", "\"");
			// Surround with code-block setup
			html = string.Concat("<div class=\"editor-colors\"><pre>", html, "</pre></div>");
			
			// Console.WriteLine(styleContent);
			return html;
		}
	}
}