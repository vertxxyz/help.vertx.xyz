using System;
using FluentAssertions.Execution;
using Xunit;

namespace Troubleshooter.Tests
{
	public class LanguageTests
	{
		/// <summary>
		/// Tests for common issues with language
		/// </summary>
		[Theory]
		[ClassData(typeof(PageData))]
		public void ValidateIncorrectLanguage(string name, string path, string text)
		{
			using var assertionScope = new AssertionScope(name);
			text.Should().NotContain("Compile Error", StringComparison.OrdinalIgnoreCase, "Should be \"Compiler Error\"");
		}
	}
}