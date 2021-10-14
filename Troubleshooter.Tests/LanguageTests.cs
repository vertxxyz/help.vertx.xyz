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
			text.Should().NotContain("compile error", StringComparison.OrdinalIgnoreCase, "Should be \"Compiler Error\"");
			text.Should().NotContain("world-space", StringComparison.OrdinalIgnoreCase, "Should be \"World space\"");
			text.Should().NotContain("local-space", StringComparison.OrdinalIgnoreCase, "Should be \"Local space\"");
			text.Should().NotContain("unity event", StringComparison.OrdinalIgnoreCase, "Should be \"UnityEvent\"");
		}
		
		/// <summary>
		/// Tests for common issues with capitalisation
		/// </summary>
		[Theory]
		[ClassData(typeof(PageData))]
		public void ValidateIncorrectCapitalisation(string name, string path, string text)
		{
			using var assertionScope = new AssertionScope(name);
			text.Should().NotContain("Game View", StringComparison.Ordinal, "Should be \"Game view\"");
			text.Should().NotContain("Scene View", StringComparison.Ordinal, "Should be \"Scene view\"");
		}
	}
}