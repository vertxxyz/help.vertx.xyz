using System;
using FluentAssertions.Execution;
using Xunit;
// ReSharper disable StringLiteralTypo

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
			text.Should().NotContain("play-mode", StringComparison.OrdinalIgnoreCase, "Should be \"Play Mode\"");
			text.Should().NotContain("edit-mode", StringComparison.OrdinalIgnoreCase, "Should be \"Edit Mode\"");
			text.Should().NotContain("unity event", StringComparison.OrdinalIgnoreCase, "Should be \"UnityEvent\"");
			text.Should().NotContain("alt+click", StringComparison.OrdinalIgnoreCase, "Should be \"hold <kbd>Alt</kbd> while clicking\"");
			text.Should().NotContain("ctrl+click", StringComparison.OrdinalIgnoreCase, "Should be \"hold <kbd>Ctrl</kbd> while clicking\"");
			text.Should().NotContain("uitoolkit", StringComparison.OrdinalIgnoreCase, "Should be \"UI Toolkit\"");
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
			text.Should().NotContain("play mode", StringComparison.Ordinal, "Should be \"Play Mode\"");
			text.Should().NotContain("edit mode", StringComparison.Ordinal, "Should be \"Edit Mode\"");
		}
	}
}