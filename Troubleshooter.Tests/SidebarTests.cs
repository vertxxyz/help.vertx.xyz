using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions.Execution;
using Xunit;

namespace Troubleshooter.Tests;

public partial class SidebarTests
{
	/// <summary>
	/// Tests for line breaks that are not preceded by two new lines.
	/// </summary>
	[Theory]
	[ClassData(typeof(SidebarData))]
	public void ValidateSidebarAnchorLinks(string name, string path, string text)
	{
		string pagePath = path.Replace("_sidebar", string.Empty);
		string? pageText = null;
		using var assertionScope = new AssertionScope();
		foreach (string link in AnchorLinksAsFullPaths(text))
		{
			string query = @$"# *{link} *\r\n";
			pageText ??= File.ReadAllText(pagePath);
			pageText.Should().Match(v => Regex.IsMatch(v, query, RegexOptions.IgnoreCase), $"\"#{link}\" anchor does not exist - \"{name}\"");
		}
	}

	[GeneratedRegex(@"]\(#([\w /%.]+)\)")]
	private static partial Regex AnchorRegex { get; }

	/// <summary>
	/// Parse markdown text looking for anchor links
	/// </summary>
	/// <param name="text">The markdown</param>
	/// <returns></returns>
	private static IEnumerable<string> AnchorLinksAsFullPaths(string text)
	{
		MatchCollection matches = AnchorRegex.Matches(text);
		for (int i = 0; i < matches.Count; i++)
		{
			Group group = matches[i].Groups[1];
			var match = group.Value;
			yield return match;
		}
	}
}
