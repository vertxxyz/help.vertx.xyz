using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static partial class SiteBuilder
	{
		private static void BuildContent(Arguments arguments, Site site)
		{
			IOUtility.CopyAll(new DirectoryInfo(site.ContentDirectory), new DirectoryInfo(arguments.Path));
			
			int siteRootIndex = GetSiteRootIndex(site, ResourceLocation.Site);
			
			// Copy all files that are not pages to the destination
			foreach (var path in Directory.EnumerateFiles(site.Directory, "*", SearchOption.AllDirectories))
			{
				string extension = Path.GetExtension(path);
				if(extension.Equals(".md")) continue; // Ignore pages
				
				string fullPath = Path.GetFullPath(path);
				string outputPath = Path.Combine(arguments.HtmlOutputDirectory, $"{ConvertFullPathToLinkPath(fullPath, siteRootIndex)}{extension}");
				IOUtility.CopyFileIfDifferent(outputPath, new FileInfo(fullPath));
			}
			
			int embedRootIndex = GetSiteRootIndex(site, ResourceLocation.Embed);
			
			// Copy all embed files that are not pages to the destination/Embeds
			foreach (var path in Directory.EnumerateFiles(site.EmbedsDirectory, "*", SearchOption.AllDirectories))
			{
				string extension = Path.GetExtension(path);
				if(extension.Equals(".md") || extension.Equals(".rtf")) continue; // Ignore pages
				
				string fullPath = Path.GetFullPath(path);
				string outputPath = Path.Combine(arguments.HtmlOutputDirectory, "Embeds", $"{ConvertFullPathToLinkPath(fullPath, embedRootIndex)}{extension}");
				IOUtility.CopyFileIfDifferent(outputPath, new FileInfo(fullPath));
			}
		}
	}
}