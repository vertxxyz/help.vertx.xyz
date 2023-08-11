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
		assertionScope.AddReportable("path", path);
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
	}

	private static readonly Regex s_UnityRegex = GetUnityRegex();
	private static readonly Regex s_UGuiRegex = GetUGuiRegex();
		
	/// <summary>
	/// Tests for common issues with capitalisation
	/// </summary>
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateIncorrectCapitalisation(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope(name);
		text.Should().NotContain("Game View", StringComparison.Ordinal, "we should write \"Game view\"");
		text.Should().NotContain("Scene View", StringComparison.Ordinal, "we should write \"Scene view\"");
		text.Should().NotContain("Project Window", StringComparison.Ordinal, "we should write \"Project window\"");
		text.Should().NotContain("play mode", StringComparison.Ordinal, "we should write \"Play Mode\"");
		text.Should().NotContain("edit mode", StringComparison.Ordinal, "we should write \"Edit Mode\"");
		text.Should().NotContain("the hub", StringComparison.Ordinal, "we should write \"the Hub\"");
		text.Should().NotContain("Unity hub", StringComparison.Ordinal, "we should write \"Unity Hub\"");
		text.Should().NotContain(".Net", StringComparison.Ordinal, "we should write \".NET\"");
		text.Should().NotContain("assembly definition", StringComparison.Ordinal, "we should write \"Assembly Definition\"");
		text.Should().NotMatchRegex(s_UGuiRegex, "we should write \"uGUI\"");
		text.Should().NotMatchRegex(s_UnityRegex, "we should write \"Unity\"");
	}

    [GeneratedRegex(@"\sunity[\s.,]")]
    private static partial Regex GetUnityRegex();
    
    [GeneratedRegex(@"\sUGUI[\s.,]")]
    private static partial Regex GetUGuiRegex();
}