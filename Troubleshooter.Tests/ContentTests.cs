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
	/// Tests that pages have at least some content in them.
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateEmptyPage(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope(name);
		text.Should().NotBeNullOrEmpty("Pages should have content");
	}

	private static readonly Regex s_footnoteRegex = FootnoteRegex();
	private static readonly Regex s_incorrectFootnoteRegex = IncorrectFootnoteRegex();

	[Flags]
	private enum FootnotePair : byte
	{
		Source = 1 << 0,
		Destination = 1 << 1,
		Both = Source | Destination
	}

	/// <summary>
	/// Tests that footnotes are correctly formatted
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateFootnotes(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope(name);
		Assert.DoesNotMatch(s_incorrectFootnoteRegex, text);
		MatchCollection footnotes = s_footnoteRegex.Matches(text);
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

			string value = match.Groups[1].Value;
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

	private static readonly Regex s_incorrectPackageDocLink = IncorrectPackageDocLink();

	/// <summary>
	/// Validates links to package docs, ensuring they have @latest links.
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidatePackageDocLinks(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope(name);
		text.Should().NotMatchRegex(s_incorrectPackageDocLink);
	}

    [GeneratedRegex(@"\[\^(\d+)\]")]
    private static partial Regex FootnoteRegex();

    [GeneratedRegex(@"\[\d+\^\]")]
    private static partial Regex IncorrectFootnoteRegex();

    [GeneratedRegex(@"@[\d.]+?/(?:api|manual)/")]
    private static partial Regex IncorrectPackageDocLink();
}
