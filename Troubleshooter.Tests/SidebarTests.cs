using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Troubleshooter.Tests
{
	public class SidebarTests
	{
		/// <summary>
		/// Tests for line breaks that are not preceded by two new lines.
		/// </summary>
		[Theory]
		[ClassData(typeof(SidebarData))]
		public void ValidateSidebarAnchorLinks(string name, string path)
		{
			string text = File.ReadAllText(path);
			var pagePath = path.Replace("_sidebar", string.Empty);
			string pageText = File.ReadAllText(pagePath);
			using (new AssertionScope())
			{
				foreach (var link in AnchorLinksAsFullPaths(text))
				{
					string query = @$"# *{link} *\r\n";
					pageText.Should().Match(v => Regex.IsMatch(v, query, RegexOptions.IgnoreCase), $"\"#{link}\" anchor does not exist - \"{name}\"");
				}
			}
		}
		
		/// <summary>
		/// Parse markdown text looking for anchor links
		/// </summary>
		/// <param name="text">The markdown</param>
		/// <returns></returns>
		public static IEnumerable<string> AnchorLinksAsFullPaths(string text)
		{
			MatchCollection matches = Regex.Matches(text, @"]\(#([\w /%.]+)\)");
			for (int i = 0; i < matches.Count; i++)
			{
				Group group = matches[i].Groups[1];
				var match = group.Value;
				yield return match;
			}
		}
	}
}