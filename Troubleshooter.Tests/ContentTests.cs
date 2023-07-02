using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Troubleshooter.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public partial class ContentTests
{
	/// <summary>
	/// Matches --- not preceded by multiple newlines
	/// </summary>
	private static readonly Regex lineBreak01Regex = LineBreak01Regex();

	/// <summary>
	/// Matches .rtf>> not followed by:
	/// 2x newline,
	/// newline + code,
	/// newline + header,
	/// newline + end of file
	/// </summary>
	private static readonly Regex lineBreak02Regex = LineBreak02Regex();

	/// <summary>
	/// Tests for line breaks that are not preceded by two new lines.
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateLineBreaks(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope(name);
		assertionScope.BecauseOf("Line breaks must be preceded by multiple newlines. Code blocks must be followed by multiple newlines.");
		text.Should().NotMatch(lineBreak01Regex);
		text.Should().NotMatch(lineBreak02Regex);
	}

	/// <summary>
	/// Tests that pages have at least some content in them.
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateEmptyPage(string name, string path, string text)
	{
		using (new AssertionScope(name))
			text.Should().NotBeNullOrEmpty("Pages should have content");
	}

	private static readonly Regex footnoteRegex = FootnoteRegex();
	private static readonly Regex incorrectFootnoteRegex = IncorrectFootnoteRegex();

	[Flags]
	private enum FootnotePair : byte
	{
		None,
		Source,
		Destination,
		Both = Source | Destination
	}

	/// <summary>
	/// Tests that footnotes are correctly formatted
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateFootnotes(string name, string path, string text)
	{
		using (new AssertionScope(name))
		{
			Assert.DoesNotMatch(incorrectFootnoteRegex, text);
			MatchCollection footnotes = footnoteRegex.Matches(text);
			Dictionary<string, FootnotePair> footnotePairs = new();
			foreach (Match match in footnotes)
			{
				int nextIndex = match.Index + match.Length;
				int prevIndex = match.Index - 1;
				FootnotePair pair;
				if (nextIndex < text.Length && text[nextIndex] == ':' && prevIndex >= 0 && text[prevIndex] == '\n')
					pair = FootnotePair.Destination;
				else
					pair = FootnotePair.Source;

				var value = match.Groups[1].Value;
				if (!footnotePairs.TryGetValue(value, out FootnotePair oldPair))
					footnotePairs.Add(value, pair);
				else
					footnotePairs[value] = oldPair | pair;
			}

			foreach (KeyValuePair<string, FootnotePair> pair in footnotePairs)
			{
				pair.Value.Should().Be(FootnotePair.Both, $"{pair.Value} does not make a pair of footnotes in {name}");
			}
		}
	}
	
	private static readonly Regex incorrectPackageDocLink = IncorrectPackageDocLink();
	
	/// <summary>
	/// Validates links to package docs, ensuring they have @latest links.
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidatePackageDocLinks(string name, string path, string text)
	{
		using (new AssertionScope(name))
		{
			Assert.DoesNotMatch(incorrectPackageDocLink, text);
		}
	}

    [GeneratedRegex(@"(?<!\r\n)\r\n---(?:\s|$)", RegexOptions.Compiled)]
    private static partial Regex LineBreak01Regex();
    
    [GeneratedRegex(@".rtf>>(?! *\r?\n\r?\n| *\r?\n<<| *\r?\n#| *\r*\n?$)", RegexOptions.Compiled)]
    private static partial Regex LineBreak02Regex();
    
    [GeneratedRegex(@"\[\^(\d+)\]", RegexOptions.Compiled)]
    private static partial Regex FootnoteRegex();
    
    [GeneratedRegex(@"\[\d+\^\]", RegexOptions.Compiled)]
    private static partial Regex IncorrectFootnoteRegex();
    
    [GeneratedRegex(@"@[\d.]+?\/(?:api|manual)\/", RegexOptions.Compiled)]
    private static partial Regex IncorrectPackageDocLink();
}