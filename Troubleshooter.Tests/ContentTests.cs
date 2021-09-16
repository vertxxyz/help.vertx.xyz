using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Troubleshooter.Tests
{
	public class ContentTests
	{
		/// <summary>
		/// Matches --- not preceded by multiple newlines
		/// </summary>
		private static readonly Regex lineBreak01Regex = new(@"(?<!\r\n)\r\n---(?:\s|$)", RegexOptions.Compiled);

		/// <summary>
		/// Matches .rtf>> not followed by:
		/// 2x newline,
		/// newline + code,
		/// newline + header,
		/// newline + end of file
		/// </summary>
		private static readonly Regex lineBreak02Regex = new(@".rtf>>(?! *\r?\n\r?\n| *\r?\n<<| *\r?\n#| *\r*\n?$)", RegexOptions.Compiled);

		/// <summary>
		/// Tests for line breaks that are not preceded by two new lines.
		/// </summary>
		[Theory]
		[ClassData(typeof(PageData))]
		public void ValidateLineBreaks(string name, string path, string text)
		{
			using var assertionScope = new AssertionScope(name);
			assertionScope.BecauseOf("Line breaks must be preceded by multiple newlines. Code blocks must be followed by multiple newlines.");
			Assert.DoesNotMatch(lineBreak01Regex, text);
			Assert.DoesNotMatch(lineBreak02Regex, text);
		}

		/// <summary>
		/// Tests that pages have at least some content in them.
		/// </summary>
		[Theory]
		[ClassData(typeof(PageData))]
		public void ValidateEmptyPage(string name, string path, string text)
		{
			using (new AssertionScope(name))
			{
				Assert.False(string.IsNullOrEmpty(text), "No page content found.");
			}
		}

		private static readonly Regex footnoteRegex = new(@"\[\^(\d+)\]", RegexOptions.Compiled);
		private static readonly Regex incorrectFootnoteRegex = new(@"\[\d+\^\]", RegexOptions.Compiled);

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
				Dictionary<string, FootnotePair> footnotePairs = new Dictionary<string, FootnotePair>();
				foreach (Match match in footnotes)
				{
					int nextIndex = match.Index + match.Length;
					FootnotePair pair;
					if (nextIndex >= text.Length || text[nextIndex] != ':')
						pair = FootnotePair.Source;
					else
						pair = FootnotePair.Destination;

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
	}
}