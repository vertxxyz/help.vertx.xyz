using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Troubleshooter.Tests;

public class PageData : IEnumerable<object[]>
{
	protected readonly HashSet<string> Symlinks;
	private readonly Site _site;

	public PageData()
	{
		_site = TestUtility.TestSite;
		Symlinks = PageUtility.GetAllSymlinkedFiles(_site.AssetsRoot);
	}

	public virtual IEnumerator<object[]> GetEnumerator()
	{
		foreach (string file in Directory.EnumerateFiles(_site.AssetsRoot, "*.md", SearchOption.AllDirectories))
		{
			// Ignore symlinked files.
			if (Symlinks.Contains(file))
				continue;

			if (file.EndsWith(Constants.GeneratorSuffix))
			{
				foreach ((string path, PageResource value) in
				         SiteBuilder.ProcessGenerators("", "", _site, null, new PageResource(file, ResourceType.Generator, ResourceLocation.Site, null, "", "", _site)))
				{
					string localPath = new System.Uri(path).LocalPath;
					yield return [Path.GetFileNameWithoutExtension(localPath), localPath, value.MarkdownText!];
				}
			}

			yield return [Path.GetFileNameWithoutExtension(file), file, File.ReadAllText(file)];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class PageDataWithSymlinks : PageData
{
	public override IEnumerator<object[]> GetEnumerator()
	{
		using IEnumerator<object[]> enumerator = base.GetEnumerator();
		foreach (object[] objects in enumerator)
			yield return [objects[0], objects[1], objects[2], Symlinks];
	}
}

public class SidebarData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator() =>
		Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, $"*{Constants.SidebarSuffix}", SearchOption.AllDirectories)
			.Select(file => new object[] { Path.GetFileNameWithoutExtension(file), file, File.ReadAllText(file) }).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class JavascriptData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator() =>
		Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, "*.js", SearchOption.AllDirectories)
			.Select(file => new object[] { File.ReadAllText(file) }).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
