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
				return ProcessAsCopyWithStyleCode(text);
			}

			return text;
		}

		private static readonly Regex whitespaceRegex = new(@"(^ +)|( +$)", RegexOptions.Compiled | RegexOptions.Multiline); // Captures whitespace at the start or end of the line
		private static readonly Regex colorRegex = new(@"[{;]color:(#\w{6});", RegexOptions.Compiled); // Captures color information only
		private static readonly Regex styleNameRegex = new(@"_style_\d+([{^]|$)", RegexOptions.Compiled); // Captures pure names (no _style_1:before, etc)
		private static readonly Regex stylePlainRegex = new(@"_style_\d+", RegexOptions.Compiled); // Captures style information for any unmatched styles
		private static readonly Regex styleItalicRegex = new("font-style:oblique;", RegexOptions.Compiled);
		private static readonly Regex spanSpacesRegex = new(@"<span>( +)</span>", RegexOptions.Compiled);
		private static readonly Regex underlineRegex = new(@"color:(#\w{6});content:""~{500}""", RegexOptions.Compiled); //Captures the content used to underline errors

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
			{ "#BDBDBD", "token punctuation" },
			{ "#FF5647", "token unknown" }
		};
		
		public static readonly Dictionary<string, string> UnderlineColorToClassLookup = new()
		{
			{ "#85C46C", "hint-underline" },
			{ "#FF5647", "error-underline" },
		};

		private static string ProcessAsCopyWithStyleCode(string text)
		{
			int styleStart = text.IndexOf("<style>", StringComparison.Ordinal);
			int styleEnd = text.IndexOf("</style>", StringComparison.Ordinal);
			string styleContent = text[(styleStart + 7)..(styleEnd - 1)];

			// Collect inline style information
			Dictionary<string, string> colorStylesToReplacement = new();
			{
				MatchCollection styles = colorRegex.Matches(styleContent);
				foreach (Match match in styles)
				{
					string color = match.Groups[1].Value.ToUpper();
					if (!ColorToClassLookup.TryGetValue(color, out var replacementStyle))
						continue;

					if (!TryGetStyleNameMatchBeforeBlockData(match, styleContent, out var styleNameMatch))
						continue;

					colorStylesToReplacement.Add(styleNameMatch.Groups[0].Value, replacementStyle);
					// Console.WriteLine($"{styleNameMatch.Groups[1].Value}, {replacementStyle}");
				}
			}

			List<string> italicStyles = new();
			{
				MatchCollection styles = styleItalicRegex.Matches(styleContent);
				foreach (Match match in styles)
				{
					if (!TryGetStyleNameMatchBeforeBlockData(match, styleContent, out var styleNameMatch))
						continue;

					italicStyles.Add(styleNameMatch.Groups[0].Value);
				}
			}
			
			{ // Underlines
				MatchCollection styles = underlineRegex.Matches(styleContent);
				foreach (Match match in styles)
				{
					string color = match.Groups[1].Value.ToUpper();
					if (!UnderlineColorToClassLookup.TryGetValue(color, out var replacementStyle))
						continue;
					
					if (!TryGetStyleNameMatchBeforeBlockData(match, styleContent, out var styleNameMatch, stylePlainRegex))
						continue;

					colorStylesToReplacement.Add(styleNameMatch.Groups[0].Value, replacementStyle); 
					// Console.WriteLine($"{styleNameMatch.Groups[0].Value}, {replacementStyle}");
				}
			}


			string content = text[text.IndexOf('<', styleEnd + 8)..(text.LastIndexOf('>', text.Length - 6) + 1)];
			StringBuilder stringBuilder = new(content);
			// Remove unnecessary styles
			stringBuilder.Replace("_style_line", string.Empty);
			stringBuilder.Replace("_style_span ", string.Empty);
			// Remove divs
			stringBuilder.Replace("<div class=\"_style_default\">", string.Empty);
			stringBuilder.Replace("/<div", string.Empty);
			stringBuilder.TrimStart();
			stringBuilder.TrimEnd();

			content = stringBuilder.ToString();
			{ // Style replacement
				// Replace italic <span>s with <em> - Must occur before we replace inline styles
				foreach (string style in italicStyles)
				{
					content = StringUtility.ReplaceMatch(content, new Regex(@$"<(\w+) [\w ""_=]+{style}["" ][^<]+(</\w+>)"), (match, builder) =>
					{
						builder.Append("<em");
						builder.Append(match.Value[(match.Groups[1].Index + match.Groups[1].Length - match.Index)..(match.Groups[2].Index - match.Index)]);
						builder.Append("</em>");
					});
				}

				// Replace inline styles with final styles.
				foreach ((string style, string replacement) in colorStylesToReplacement)
					content = Regex.Replace(content, $"{style}(?=[\" ])", replacement);
			}

			// Remove all un-processed styles.
			stringBuilder = new StringBuilder(stylePlainRegex.Replace(content, string.Empty));
			// Remove empty classes
			stringBuilder.Replace(" class=\"\"", string.Empty);
			stringBuilder.Replace(" class=\" \"", string.Empty);
			// Remove paragraphs
			stringBuilder.Replace("<p>", string.Empty);
			stringBuilder.Replace("</p>", string.Empty);

			string html = stringBuilder.ToString();
			html = whitespaceRegex.Replace(html, string.Empty);
			html = html.Replace($"<span></span>{Environment.NewLine}", Environment.NewLine);
			html = html.Replace($"{Environment.NewLine}{Environment.NewLine}", "<br>");
			// Cleanup empty spaces in classes
			html = html.Replace("\" ", "\"");
			html = html.Replace(" \"", "\"");
			// Replace spans containing only spaces with just the spaces.
			html = StringUtility.ReplaceMatch(html, spanSpacesRegex, (s, builder) => builder.Append(s), 1);
			// Surround with code-block setup
			html = string.Concat("<div class=\"editor-colors\"><pre>", html, "</pre></div>");

			// Console.WriteLine(styleContent);
			return html;
		}

		/// <summary>
		/// In a block like ._style_1{foo;bar;}, find "_style_1" from a match of either foo; or bar;
		/// </summary>
		private static bool TryGetStyleNameMatchBeforeBlockData(Match blockMatch, string content, out Match result)
			=> TryGetStyleNameMatchBeforeBlockData(blockMatch, content, out result, styleNameRegex);
		
		/// <summary>
		/// In a block like ._style_1{foo;bar;}, find "_style_1" from a match of either foo; or bar;
		/// </summary>
		private static bool TryGetStyleNameMatchBeforeBlockData(Match blockMatch, string content, out Match result, Regex styleNameRegex)
		{
			int blockStart = content.LastIndexOf('{', blockMatch.Index + 1);
			int styleNameStart = content.LastIndexOf('.', blockStart);
			/*string styleName = styleContent[(styleNameStart + 1)..blockStart];
			Console.WriteLine(styleName);*/
			result = styleNameRegex.Match(content, styleNameStart + 1, blockStart - (styleNameStart + 1));
			return result.Success;
		}
	}
}