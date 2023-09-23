using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;

namespace Troubleshooter.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public partial class LinkTests
{
	private readonly ITestOutputHelper _testOutputHelper;
	private readonly HashSet<string> _embeddedFiles = new();

	public LinkTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
		string embedsRoot = TestUtility.TestSite.EmbedsDirectory;
		foreach (string embeddedFile in Directory.EnumerateFiles(embedsRoot, "*", SearchOption.AllDirectories))
			_embeddedFiles.Add(embeddedFile[(embedsRoot.Length + 1)..].ToWorkingPath());
	}

	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateLinks(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope();
		string siteRoot = TestUtility.TestSite.Directory;
		foreach ((string fullPath, _) in PageUtility.GetLinkFullPathsFromMarkdownText(text, path, siteRoot))
			new FileInfo(fullPath).Should().Exist("\"{0}\" is missing a link", path);
	}

	[Fact]
	public void ValidateCollectedSymlinks()
	{
		Dictionary<string,string> lookup = PageUtility.CollectSymlinkedFilesLookup(TestUtility.TestSite.Directory);
		using var assertionScope = new AssertionScope();
		foreach ((string from, string to) in lookup)
		{
			new FileInfo(from).Should().Exist("\"from\" is missing");
			new FileInfo(to).Should().Exist("\"to\" is missing");
		}

		_testOutputHelper.WriteLine(lookup.ToElementsString(kvp => $"\"{kvp.Key}\" -> \"{kvp.Value}\""));
	}

	[GeneratedRegex(@"(?<!!)\[.+?\]\((.+?)\)", RegexOptions.Compiled)]
	private static partial Regex GetMarkdownLinkRegex();

	private static readonly Regex s_markdownLink = GetMarkdownLinkRegex();

	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateLinkContent(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope();
		MatchCollection matches = s_markdownLink.Matches(text);
		foreach (Match match in matches)
		{
			match.Groups[1].Value.Should().NotContain(' ', StringComparison.Ordinal);
		}
	}

	[Theory]
	[ClassData(typeof(PageDataWithSymlinks))]
	public void ValidateLinksAreNotToSymlink(string name, string path, string text, HashSet<string> symlinks)
	{
		using var assertionScope = new AssertionScope();
		string siteRoot = TestUtility.TestSite.Directory;
		foreach ((string fullPath, _) in PageUtility.GetLinkFullPathsFromMarkdownText(text, path, siteRoot))
		{
			symlinks.Should().NotContain(fullPath, "\"{0}\" shouldn't reference a symlink file", path);
		}
	}

	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateEmbeds(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope();
		foreach ((string localPath, _) in PageUtility.GetEmbedsAsLocalPathsFromMarkdownText(text))
			_embeddedFiles.Should().Contain(localPath, $"was not present in embedded files - \"{name}\"");
	}

	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateImages(string name, string path, string text)
	{
		using var assertionScope = new AssertionScope();
		string directory = Path.GetDirectoryName(path)!;
		foreach ((string localPath, _) in PageUtility.GetImagesAsLocalPathsFromMarkdownText(text, false))
		{
			string fullPath = localPath.StartsWith('/') // path is not finalised
				? Path.GetFullPath(Path.Combine(TestUtility.TestSite.ContentDirectory, localPath[1..])).ToUnTokenized()
				: Path.GetFullPath(Path.Combine(directory, localPath)).ToUnTokenized();

			new FileInfo(fullPath).Should().Exist();
		}
	}
}
