using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static partial class SiteBuilder
	{
		private static void BuildContent(Arguments arguments, Site site)
		{
			// Copy content to destination
			IOUtility.CopyAll(new DirectoryInfo(site.ContentDirectory), new DirectoryInfo(arguments.Path));
			
			int siteRootIndex = GetSiteRootIndex(site, ResourceLocation.Site);

			int siteContent = 0;
			int totalContent = 0;
			
			// Copy all files that are not pages to the destination
			foreach (var path in Directory.EnumerateFiles(site.Directory, "*", SearchOption.AllDirectories))
			{
				string extension = Path.GetExtension(path);
				if(extension.Equals(".md")) continue; // Ignore pages
				
				string fullPath = Path.GetFullPath(path);
				string outputPath = ConvertRootFullEmbedPathToLinkPath(fullPath, extension, siteRootIndex, arguments);

				totalContent++;
				if (IOUtility.CopyFileIfDifferent(outputPath, new FileInfo(fullPath)))
					siteContent++;
			}


			int embedContent = 0;
			int embedRootIndex = GetSiteRootIndex(site, ResourceLocation.Embed);
			
			// Copy all embed files that are not pages to the destination/Embeds
			foreach (var path in Directory.EnumerateFiles(site.EmbedsDirectory, "*", SearchOption.AllDirectories))
			{
				string extension = Path.GetExtension(path);
				if(extension.Equals(".md") || extension.Equals(".rtf")) continue; // Ignore pages
				
				string fullPath = Path.GetFullPath(path);
				string outputPath = ConvertFullEmbedPathToLinkPath(fullPath, extension, embedRootIndex, arguments);
				
				totalContent++;
				if(IOUtility.CopyFileIfDifferent(outputPath, new FileInfo(fullPath)))
					embedContent++;
			}
			
			arguments.VerboseLog($"{siteContent + embedContent} content files were written to disk. ({totalContent} total)");
		}
	}
}