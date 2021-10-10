using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Troubleshooter.Tests
{
	public class PageData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator() =>
			Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, "*.md", SearchOption.AllDirectories)
				.Select(file => new object[]
				{
					Path.GetFileNameWithoutExtension(file),
					file,
					File.ReadAllText(file)
				}).GetEnumerator();

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
}