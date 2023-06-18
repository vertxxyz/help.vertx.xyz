using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Troubleshooter.Tests;

public class PageData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		foreach (object[] objects in Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, "*.md", SearchOption.AllDirectories)
			         .Select(file => new object[]
			         {
				         Path.GetFileNameWithoutExtension(file),
				         file,
				         File.ReadAllText(file)
			         }))
			yield return objects;

		foreach (string file in Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, "*.md.gen", SearchOption.AllDirectories))
		{
			foreach ((string path, PageResource value) in SiteBuilder.ProcessGenerator(TestUtility.TestSite, null,
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
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class SidebarData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator() =>
		Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, "*_sidebar.md", SearchOption.AllDirectories)
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