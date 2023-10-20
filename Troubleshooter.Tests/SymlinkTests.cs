using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;

namespace Troubleshooter.Tests;

public class SymlinkTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	public SymlinkTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

	[Fact]
	public void CheckForSymlinksInMainSite()
	{
		Dictionary<string,string> lookup = PageUtility.CollectSymlinkedFilesLookup(TestUtility.TestSite.Directory);
		using var assertionScope = new AssertionScope();
		lookup.Should().BeEmpty("the main \"Site\" directory should not contain any symlinks. The \"Site Redirects\" directory contains these redirects.");
	}

	[Fact]
	public void ValidateCollectedSymlinks()
	{
		Dictionary<string,string> lookup = PageUtility.CollectSymlinkedFilesLookup(TestUtility.TestSite.RedirectsDirectory);
		using var assertionScope = new AssertionScope();
		foreach ((string from, string to) in lookup)
		{
			new FileInfo(from).Should().Exist("\"from\" is missing");
			new FileInfo(to).Should().Exist("\"to\" is missing");
		}

		_testOutputHelper.WriteLine(lookup.ToElementsString(kvp => $"\"{kvp.Key}\" -> \"{kvp.Value}\""));
	}

	[Fact]
	public void CheckForDestroyedSymlinks()
	{
		foreach (string entry in Directory.EnumerateFileSystemEntries(TestUtility.TestSite.RedirectsDirectory, "*", SearchOption.AllDirectories))
		{
			string extension = Path.GetExtension(entry);
			if (extension == "")
			{
				DirectoryInfo directory = new(entry);
				directory.Should().Exist("File without extension found. This is likely a broken symlink.");
			}
			else
			{
				FileInfo file = new(entry);
				if (file.LinkTarget == null)
				{
					continue;
				}
				_testOutputHelper.WriteLine($"{file.Length} length");
			}
		}
	}
}
