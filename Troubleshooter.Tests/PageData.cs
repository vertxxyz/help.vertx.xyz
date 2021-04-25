using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Troubleshooter.Tests
{
	public class PageData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator() =>
			Directory.EnumerateFiles(TestUtility.TestSite.AssetsRoot, "*.md", SearchOption.AllDirectories).Select(file => new object[] {Path.GetFileNameWithoutExtension(file), file}).GetEnumerator();
		
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}