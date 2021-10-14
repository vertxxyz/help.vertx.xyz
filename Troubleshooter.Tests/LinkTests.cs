using System.Collections.Generic;
using System.IO;
using System.Web;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;

namespace Troubleshooter.Tests
{
	public class LinkTests
	{
		private readonly ITestOutputHelper testOutputHelper;
		private readonly HashSet<string> embeddedFiles = new();

		public LinkTests(ITestOutputHelper testOutputHelper)
		{
			this.testOutputHelper = testOutputHelper;
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
				new FileInfo(fullPath).Should().Exist($"\"{name}\" is missing a link");
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
				string directory = Path.GetDirectoryName(path);
				foreach ((string localPath, _) in PageUtility.LocalImagesAsRootPaths(text, false))
				{
					string fullPath = Path.GetFullPath(Path.Combine(directory, localPath)).ToUnTokenized();
					new FileInfo(fullPath).Should().Exist();
				}
			}
		}
	}
}