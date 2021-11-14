using System;
using System.IO;
using System.Linq;
using Troubleshooter.Constants;

namespace Troubleshooter;

public static class SiteLogging
{
	public static void LogAllExternalUrls(Arguments arguments)
	{
		Site site = new Site(arguments.TroubleshooterRoot);
		foreach (string file in Directory.EnumerateFiles(site.AssetsRoot, "*.md", SearchOption.AllDirectories))
		{
			string text = File.ReadAllText(file);
			string[] urls = PageUtility.ExternalLinkUrls(text).Select(u => u.url).ToArray();
			if (urls.Length == 0)
				continue;

			Console.WriteLine(file);
			foreach (string url in urls)
			{
				Console.Write('\t');
				Console.WriteLine(url);
			}
		}
	}
}