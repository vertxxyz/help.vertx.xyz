using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentAssertions.Execution;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace Troubleshooter.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public partial class LanguageTests
{
	/// <summary>
	/// Tests for common issues with language
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateIncorrectLanguage(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope(name);
		assertionScope.AppendTracing(path);
		text.Should().NotContain("compile error", StringComparison.OrdinalIgnoreCase, "we should use \"Compiler Error\"");
		text.Should().NotContain("world-space", StringComparison.OrdinalIgnoreCase, "we should use \"World space\"");
		text.Should().NotContain("local-space", StringComparison.OrdinalIgnoreCase, "we should use \"Local space\"");
		text.Should().NotContain("play-mode", StringComparison.OrdinalIgnoreCase, "we should use \"Play Mode\"");
		text.Should().NotContain("edit-mode", StringComparison.OrdinalIgnoreCase, "we should use \"Edit Mode\"");
		text.Should().NotContain("unity event", StringComparison.OrdinalIgnoreCase, "we should use \"UnityEvent\"");
		text.Should().NotContain("alt+click", StringComparison.OrdinalIgnoreCase, "we should use \"hold <kbd>Alt</kbd> while clicking\"");
		text.Should().NotContain("ctrl+click", StringComparison.OrdinalIgnoreCase, "we should use \"hold <kbd>Ctrl</kbd> while clicking\"");
		text.Should().NotContain("uitoolkit", StringComparison.OrdinalIgnoreCase, "we should use \"UI Toolkit\"");
		text.Should().NotContain("assmdef", StringComparison.OrdinalIgnoreCase, "we should use \"asmdef\"");
		text.Should().NotContain("left hand", StringComparison.OrdinalIgnoreCase, "we should use \"left-hand\"");
		text.Should().NotContain("right hand", StringComparison.OrdinalIgnoreCase, "we should use \"right-hand\"");
		text.Should().NotContain("framerate", StringComparison.OrdinalIgnoreCase, "we should use \"frame rate\"");
		text.Should().NotContain("eg.", StringComparison.OrdinalIgnoreCase, "we should use \"for example\"");
		text.Should().NotContain("etc.", StringComparison.OrdinalIgnoreCase, "we should use \"such as\", or \"like\", \"and other X\"");
		text.Should().NotContain("ie.", StringComparison.OrdinalIgnoreCase, "we should use \"that is\", or \"such as\"");
		text.Should().NotContain("i.e.", StringComparison.OrdinalIgnoreCase, "we should use \"that is\", or \"such as\"");
		text.Should().NotContain("double check", StringComparison.OrdinalIgnoreCase, "we should use \"double-check\"");
		text.Should().NotContain("text mesh pro", StringComparison.OrdinalIgnoreCase, "we should use \"TextMesh Pro\"");
		text.Should().NotContain("project view", StringComparison.OrdinalIgnoreCase, "we should use \"Project window\"");
		text.Should().NotMatchRegex(MacRegex, "we should write \"macOS\"");
	}

	[GeneratedRegex(@"\b(?<!(com\.|\*\.))unity(?!(\.com|yaml|\.huh|\.html))[\s.,]")]
	private static partial Regex UnityRegex { get; }

	[GeneratedRegex(@"\bUGUI(?!\.md)[\s.,]")]
	private static partial Regex UGuiRegex { get; }

	[GeneratedRegex(@"\bgameobjects?(?!\.html)[\s.,]")]
	private static partial Regex GameObjectRegex { get; }

	[GeneratedRegex(@"\buss[\s.,]")]
	private static partial Regex UssRegex { get; }

	[GeneratedRegex(@"\bmac[\s.,]", RegexOptions.IgnoreCase)]
	private static partial Regex MacRegex { get; }

	[GeneratedRegex(@"\bplay mode[\s.,]")]
	private static partial Regex PlayModeRegex { get; }

	[GeneratedRegex(@"\bUI Toolkit debugger[\s.,]")]
	private static partial Regex UiToolkitDebuggerRegex { get; }

	/// <summary>
	/// Tests for common issues with capitalisation
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateIncorrectCapitalisation(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope(name);
		text.Should().NotContain("Game View", StringComparison.Ordinal, "we should write \"Game view\"");
		text.Should().NotContain("game view", StringComparison.Ordinal, "we should write \"Game view\"");
		text.Should().NotContain("Scene View", StringComparison.Ordinal, "we should write \"Scene view\"");
		text.Should().NotContain("Project Window", StringComparison.Ordinal, "we should write \"Project window\"");
		text.Should().NotContain("Play mode", StringComparison.Ordinal, "we should write \"Play Mode\"");
		text.Should().NotMatchRegex(PlayModeRegex, "we should write \"Play Mode\""); // Display mode is a thing, so regex is required.
		text.Should().NotContain("edit mode", StringComparison.Ordinal, "we should write \"Edit Mode\"");
		text.Should().NotContain("Debug Mode", StringComparison.Ordinal, "we should write \"Debug mode\"");
		text.Should().NotContain("the hub", StringComparison.Ordinal, "we should write \"the Hub\"");
		text.Should().NotContain("Unity hub", StringComparison.Ordinal, "we should write \"Unity Hub\"");
		text.Should().NotContain(".Net", StringComparison.Ordinal, "we should write \".NET\"");
		text.Should().NotContain("assembly definition", StringComparison.Ordinal, "we should write \"Assembly Definition\"");
		text.Should().NotContain("MacOS", StringComparison.Ordinal, "we should write \"macOS\"");
		text.Should().NotMatchRegex(UGuiRegex, "we should write \"uGUI\"");
		text.Should().NotMatchRegex(UnityRegex, "we should write \"Unity\"");
		text.Should().NotMatchRegex(GameObjectRegex, "we should write \"GameObject\"");
		text.Should().NotMatchRegex(UssRegex, "we should write \"USS\"");
		text.Should().NotMatchRegex(UiToolkitDebuggerRegex, "we should write \"UI Toolkit Debugger\"");
	}
}
