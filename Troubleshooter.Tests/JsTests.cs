using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Troubleshooter.Tests
{
	public class JsTests
	{
		private readonly ITestOutputHelper testOutputHelper;
		private static readonly Regex importRegex = new("^import .+? from [\"\'](.+?)[\"\'];\r*\n?$", RegexOptions.Compiled | RegexOptions.Multiline);
		public JsTests(ITestOutputHelper testOutputHelper)
		{
			this.testOutputHelper = testOutputHelper;
		}

		/// <summary>
		/// Ensures that imports end in ".js". Rider automatically imports without the extension, which is a runtime failure.
		/// </summary>
		[Theory]
		[ClassData(typeof(JavascriptData))]
		public void ValidateImports(string text)
		{
			foreach (Match match in importRegex.Matches(text))
			{
				testOutputHelper.WriteLine(match.Groups[1].Value);
				match.Groups[1].Value.Should().EndWith(".js");
			}
		}
	}
}