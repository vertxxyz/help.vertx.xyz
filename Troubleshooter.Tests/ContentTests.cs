using System.IO;
using System.Text.RegularExpressions;
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
		public void ValidateLineBreaks(string name, string path)
		{
			string text = File.ReadAllText(path);
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
		public void ValidateEmptyPage(string name, string path)
		{
			string text = File.ReadAllText(path);
			using (new AssertionScope(name))
			{
				Assert.False(string.IsNullOrEmpty(text), "No page content found.");
			}
		}
	}
}