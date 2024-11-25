using System.Text.RegularExpressions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;

namespace Troubleshooter.Tests;

public partial class JsTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	[GeneratedRegex("^import .+? from [\"'](.+?)[\"'];\r*\n?$", RegexOptions.Multiline)]
	private static partial Regex ImportRegex { get; }

	public JsTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

	/// <summary>
	/// Ensures that imports end in ".js". Rider automatically imports without the extension, which is a runtime failure.
	/// </summary>
	[Theory]
	[ClassData(typeof(JavascriptData))]
	public void ValidateImports(string text)
	{
		using var assertionScope = new AssertionScope();
		foreach (Match match in ImportRegex.Matches(text))
		{
			_testOutputHelper.WriteLine(match.Groups[1].Value);
			match.Groups[1].Value.Should().EndWith(".js");
		}
	}
}
