using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AdvancedStringBuilder;
using Markdig.Renderers;

namespace Troubleshooter;

public static partial class HtmlUtility
{
	public static string Parse(string text)
	{
		if (text.Contains("code_block_"))
		{
			// Text is copied using "Copy With Style" plugin and needs to be processed as code.
			return ProcessAsCopyWithStyleCode(text);
		}

		if (text.Contains("code_block"))
		{
			// Code block is generic, not using the "Copy With Style" plugin.
			return ProcessAsGenericCode(text);
		}

		return text;
	}

	private static readonly Regex whitespaceRegex = GetWhitespaceRegex(); // Captures whitespace at the start or end of the line
	private static readonly Regex colorRegex = GetColorRegex(); // Captures color information only
	private static readonly Regex styleNameRegex = GetStyleNameRegex(); // Captures pure names (no _style_1:before, etc)
	private static readonly Regex stylePlainRegex = GetStylePlainRegex(); // Captures style information for any unmatched styles
	private static readonly Regex styleItalicRegex = GetStyleItalicRegex();
	private static readonly Regex spanSpacesRegex = GetSpanSpacesRegex();
	private static readonly Regex underlineRegex = GetUnderlineRegex(); //Captures the content used to underline errors

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

		{
			// Underlines
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
		stringBuilder.Replace("</div>", string.Empty);
		stringBuilder.TrimStart();
		stringBuilder.TrimEnd();

		content = stringBuilder.ToString();
		{
			// Style replacement
			// Replace italic <span>s with <em> - Must occur before we replace inline styles
			foreach (string style in italicStyles)
			{
				content = StringUtility.ReplaceMatch(content, new Regex($"""<(\w+) [\w "_=]+{style}[" ][^<]+(</\w+>)"""), (match, builder) =>
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
		stringBuilder = new(stylePlainRegex.Replace(content, string.Empty));
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
		html = html.Replace("\" >", "\">");
		html = html.Replace(" \"", "\"");
		// Replace spans containing only spaces with just the spaces.
		html = StringUtility.ReplaceMatch(html, spanSpacesRegex, (s, builder) => builder.Append(s), 1);
		// Surround with code-block setup
		StringBuilder sb = AppendWithCodeBlockSetup(html);
		// Console.WriteLine(styleContent);
		return sb.ToString();
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

	private static string ProcessAsGenericCode(string text)
	{
		return AppendWithCodeBlockSetup(text.Replace("<div id=\"code_block\">", string.Empty).Replace("</div>", string.Empty).Trim()).ToString();
	}

	public static StringBuilder AppendWithCodeBlockSetup(string html, bool appendPre = true)
	{
		var sb = new StringBuilder(512);
		sb.Append("<div class=\"code-container\">");
		{
			sb.Append("<div class=\"dropdown code-setting\">");
			{
				// sb.Append("...");
				sb.Append("<span class=\"dropdown-caret\"></span>");
				sb.Append("<div class=\"dropdown-content\">");
				{
					sb.Append("<button class=\"code-setting-ligatures\">Ligatures ✓</button>");
					sb.Append("<button class=\"code-setting-theme\">Switch Theme</button>");
					sb.Append("<button class=\"code-setting-copy\">Copy As Text</button>");
				}
				sb.Append("</div>");
			}
			sb.Append("</div>");

			sb.Append("<div class=\"code-container-inner\">");
			{
				if (appendPre)
				{
					sb.Append($"<pre>{html}</pre>");
				}
				else
				{
					sb.Append(html);
				}
			}
			sb.Append("</div>");
		}
		sb.Append("</div>");
		return sb;
	}

	public static void AppendWithCodeBlockSetup(HtmlRenderer renderer, Action innerBlock)
	{
		renderer.Write("<div class=\"code-container\">");
		{
			renderer.Write("<div class=\"dropdown code-setting\">");
			{
				// sb.Append("...");
				renderer.Write("<span class=\"dropdown-caret\"></span>");
				renderer.Write("<div class=\"dropdown-content\">");
				{
					renderer.Write("<button class=\"code-setting-ligatures\">Ligatures ✓</button>");
					renderer.Write("<button class=\"code-setting-theme\">Switch Theme</button>");
					renderer.Write("<button class=\"code-setting-copy\">Copy As Text</button>");
				}
				renderer.Write("</div>");
			}
			renderer.Write("</div>");

			renderer.Write("<div class=\"code-container-inner\">");
			{
				innerBlock();
			}
			renderer.Write("</div>");
		}
		renderer.Write("</div>");
	}

	[GeneratedRegex("(^ +)|( +$)", RegexOptions.Multiline | RegexOptions.Compiled)]
	private static partial Regex GetWhitespaceRegex();

	[GeneratedRegex("[{;]color:(#\\w{6});", RegexOptions.Compiled)]
	private static partial Regex GetColorRegex();

	[GeneratedRegex("_style_\\d+([{^]|$)", RegexOptions.Compiled)]
	private static partial Regex GetStyleNameRegex();

	[GeneratedRegex("_style_\\d+", RegexOptions.Compiled)]
	private static partial Regex GetStylePlainRegex();

	[GeneratedRegex("font-style:oblique;", RegexOptions.Compiled)]
	private static partial Regex GetStyleItalicRegex();

	[GeneratedRegex("<span>( +)</span>", RegexOptions.Compiled)]
	private static partial Regex GetSpanSpacesRegex();

	[GeneratedRegex("color:(#\\w{6});content:\"~{500}\"", RegexOptions.Compiled)]
	private static partial Regex GetUnderlineRegex();
}