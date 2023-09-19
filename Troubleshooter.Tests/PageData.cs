using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Troubleshooter.Tests;

public class PageData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		foreach (string file in Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, "*.md", SearchOption.AllDirectories))
		{
			if (file.EndsWith(Constants.GeneratorSuffix))
			{
				foreach ((string path, PageResource value) in SiteBuilder.ProcessGenerators(TestUtility.TestSite, null,
					         new PageResource(file, ResourceType.Generator, ResourceLocation.Site)))
				{
					string localPath = new System.Uri(path).LocalPath;
					yield return new object[]
					{
						Path.GetFileNameWithoutExtension(localPath),
						localPath,
						value.MarkdownText!
					};
				}
			}
				         
			yield return new object[]
			{
				Path.GetFileNameWithoutExtension(file),
				file,
				File.ReadAllText(file)
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class SidebarData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator() =>
		Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, $"*{Constants.SidebarSuffix}", SearchOption.AllDirectories)
			.Select(file => new object[]
			{
				Path.GetFileNameWithoutExtension(file),
				file,
				File.ReadAllText(file)
			}).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class JavascriptData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator() =>
		Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, "*.js", SearchOption.AllDirectories)
			.Select(file => new object[]
			{
				File.ReadAllText(file)
			}).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}