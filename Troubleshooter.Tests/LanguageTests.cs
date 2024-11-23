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
		text.Should().NotMatchRegex(s_macRegex, "we should write \"macOS\"");
	}

	private static readonly Regex s_unityRegex = GetUnityRegex();
	private static readonly Regex s_uGuiRegex = GetUGuiRegex();
	private static readonly Regex s_gameObjectRegex = GetGameObjectRegex();
	private static readonly Regex s_ussRegex = GetUssRegex();
	private static readonly Regex s_macRegex = GetMacRegex();
	private static readonly Regex s_playModeRegex = GetPlayModeRegex();
	private static readonly Regex s_uiToolkitDebuggerRegex = GetUiToolkitDebuggerRegex();

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
		text.Should().NotMatchRegex(s_playModeRegex, "we should write \"Play Mode\""); // Display mode is a thing, so regex is required.
		text.Should().NotContain("edit mode", StringComparison.Ordinal, "we should write \"Edit Mode\"");
		text.Should().NotContain("Debug Mode", StringComparison.Ordinal, "we should write \"Debug mode\"");
		text.Should().NotContain("the hub", StringComparison.Ordinal, "we should write \"the Hub\"");
		text.Should().NotContain("Unity hub", StringComparison.Ordinal, "we should write \"Unity Hub\"");
		text.Should().NotContain(".Net", StringComparison.Ordinal, "we should write \".NET\"");
		text.Should().NotContain("assembly definition", StringComparison.Ordinal, "we should write \"Assembly Definition\"");
		text.Should().NotContain("MacOS", StringComparison.Ordinal, "we should write \"macOS\"");
		text.Should().NotMatchRegex(s_uGuiRegex, "we should write \"uGUI\"");
		text.Should().NotMatchRegex(s_unityRegex, "we should write \"Unity\"");
		text.Should().NotMatchRegex(s_gameObjectRegex, "we should write \"GameObject\"");
		text.Should().NotMatchRegex(s_ussRegex, "we should write \"USS\"");
		text.Should().NotMatchRegex(s_uiToolkitDebuggerRegex, "we should write \"UI Toolkit Debugger\"");
	}

    [GeneratedRegex(@"\b(?<!(com\.|\*\.))unity(?!(\.com|yaml|\.huh|\.html))[\s.,]")]
    private static partial Regex GetUnityRegex();

    [GeneratedRegex(@"\bgameobjects?(?!\.html)[\s.,]")]
    private static partial Regex GetGameObjectRegex();

    [GeneratedRegex(@"\bUGUI(?!\.md)[\s.,]")]
    private static partial Regex GetUGuiRegex();

    [GeneratedRegex(@"\bUI Toolkit debugger[\s.,]")]
    private static partial Regex GetUiToolkitDebuggerRegex();

    [GeneratedRegex(@"\bplay mode[\s.,]")]
    private static partial Regex GetPlayModeRegex();

    [GeneratedRegex(@"\buss[\s.,]")]
    private static partial Regex GetUssRegex();

    [GeneratedRegex(@"\bmac[\s.,]", RegexOptions.IgnoreCase)]
    private static partial Regex GetMacRegex();
}
