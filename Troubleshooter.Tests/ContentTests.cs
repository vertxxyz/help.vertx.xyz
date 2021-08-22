using System.IO;
using FluentAssertions.Execution;
using Xunit;

namespace Troubleshooter.Tests
{
	public class ContentTests
	{
		/// <summary>
		/// Tests for line breaks that are not preceded by two new lines.
		/// </summary>
		[Theory]
		[ClassData(typeof(PageData))]
		public void ValidateLineBreaks(string name, string path)
		{
			string text = File.ReadAllText(path);
			using (new AssertionScope(name))
			{
				Assert.DoesNotMatch(@"(?<!\r\n)\r\n---(?:\s|$)", text);
			}
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