using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Troubleshooter.Tests;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public class LinkTests
{
	private readonly HashSet<string> embeddedFiles = new();

	public LinkTests()
	{
		string embedsRoot = TestUtility.TestSite.EmbedsDirectory;
		foreach (string embeddedFile in Directory.EnumerateFiles(embedsRoot, "*", SearchOption.AllDirectories))
			embeddedFiles.Add(embeddedFile[(embedsRoot.Length + 1)..].ToConsistentPath());
	}

	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateLinks(string name, string path, string text)
	{
		using var scope = new AssertionScope();
		foreach ((string fullPath, _) in PageUtility.LinksAsFullPaths(text, path))
		{
			new FileInfo(fullPath).Should().Exist("{0} is missing a link", name);
		}
	}

	private static readonly Regex markdownLink = new (@"\[.+?\]\((.+?)\)", RegexOptions.Compiled);
	
	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateLinkContent(string name, string path, string text)
	{
		using var scope = new AssertionScope();
		MatchCollection matches = markdownLink.Matches(text);
		foreach (Match match in matches)
		{
			match.Groups[1].Value.Should().NotContain(' ', StringComparison.Ordinal);
		}
	}

	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateEmbeds(string name, string path, string text)
	{
		using (new AssertionScope())
		{
			foreach ((string localPath, _) in PageUtility.EmbedsAsLocalEmbedPaths(text))
				embeddedFiles.Should().Contain(localPath, $"was not present in embedded files - \"{name}\"");
		}
	}

	[Theory]
	[ClassData(typeof(PageData))]
	public void ValidateImages(string name, string path, string text)
	{
		using (new AssertionScope())
		{
			string directory = Path.GetDirectoryName(path)!;
			foreach ((string localPath, _) in PageUtility.LocalImagesAsRootPaths(text, false))
			{
				string fullPath = Path.GetFullPath(Path.Combine(directory, localPath)).ToUnTokenized();
				new FileInfo(fullPath).Should().Exist();
			}
		}
	}
}