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
		private readonly HashSet<string> embeddedFiles = new HashSet<string>();

		public LinkTests(ITestOutputHelper testOutputHelper)
		{
			this.testOutputHelper = testOutputHelper;
			var embedsRoot = TestUtility.TestSite.EmbedsDirectory;
			foreach (string embeddedFile in Directory.EnumerateFiles(embedsRoot, "*", SearchOption.AllDirectories))
				embeddedFiles.Add(embeddedFile[(embedsRoot.Length + 1)..]);
		}
		
		[Theory]
		[ClassData(typeof(PageData))]
		public void ValidateLinks(string name, string path)
		{
			string text = File.ReadAllText(path);
			using (new AssertionScope())
			{
				foreach (var link in PageUtility.LinksAsFullPaths(text, path))
					File.Exists(link.fullPath).Should().BeTrue($"\"{link.fullPath}\" does not exist - \"{name}\"");
			}
		}

		[Theory]
		[ClassData(typeof(PageData))]
		public void ValidateEmbeds(string name, string path)
		{
			string text = File.ReadAllText(path);
			using (new AssertionScope())
			{
				foreach (var embed in PageUtility.EmbedsAsLocalEmbedPaths(text))
					embeddedFiles.Should().Contain(embed.localPath, $"was not present in embedded files - \"{name}\"");
			}
		}
		
		[Theory]
		[ClassData(typeof(PageData))]
		public void ValidateImages(string name, string path)
		{
			string text = File.ReadAllText(path);
			using (new AssertionScope())
			{
				string directory = Path.GetDirectoryName(path);
				foreach (var image in PageUtility.ImagesAsRootPaths(text))
					testOutputHelper.WriteLine(HttpUtility.UrlPathEncode(Path.Combine(directory, image.localPath)));
			}
		}
	}
}