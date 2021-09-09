using System;
using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static partial class SiteBuilder
	{		
		public static int GetSiteRootIndex(Site site, ResourceLocation resourceLocation)
		{
			string directory = resourceLocation switch
			{
				ResourceLocation.Embed => site.EmbedsDirectory,
				ResourceLocation.Site => site.Directory,
				_ => throw new ArgumentOutOfRangeException(nameof(resourceLocation), resourceLocation, null)
			};

			string rootDirectory = Path.GetFullPath(directory);
			int siteRootIndex = rootDirectory.Length + 1;
			return siteRootIndex;
		}

		public static string ConvertRootFullEmbedPathToLinkPath(string fullPath, string extension, int siteRootIndex, Arguments arguments)
		{
			(string fileNameWithoutExtension, string fullPathWithoutFileName) = ProcessPath(fullPath, extension);
			string outputPath = Path.Combine(arguments.HtmlOutputDirectory, ConvertFullPathToLinkPath(fullPathWithoutFileName, siteRootIndex), $"{fileNameWithoutExtension}{extension}");
			return outputPath;
		}
		
		public static string ConvertFullEmbedPathToLinkPath(string fullPath, string extension, int siteRootIndex, Arguments arguments)
		{
			(string fileNameWithoutExtension, string fullPathWithoutFileName) = ProcessPath(fullPath, extension);
			string outputPath = Path.Combine(arguments.HtmlOutputDirectory, "Embeds", ConvertFullPathToLinkPath(fullPathWithoutFileName, siteRootIndex), $"{fileNameWithoutExtension}{extension}");
			return outputPath;
		}

		private static (string fileNameWithoutExtension, string fullPathWithoutFileName) ProcessPath(string fullPath, string extension)
		{
			string fileNameWithExtension = Path.GetFileName(fullPath);
			return (fileNameWithExtension[..^extension.Length], fullPath[..^fileNameWithExtension.Length]);
		}
		
		public static string ConvertFullPathToLinkPath(string fullPath, int siteRootIndex)
		{
			string localPath = ConvertFullPathToLocalPath(fullPath, siteRootIndex);
			return Path.ChangeExtension(localPath, null);
		}
		
		public static string ConvertFullPathToLocalPath(string fullPath, int siteRootIndex)
		{
			string localPath = fullPath[siteRootIndex..];
			/*localPath = localPath.Replace(' ', '-');*/
			localPath = localPath.Replace("&", "and");
			localPath = localPath.Replace('\\', '/');
			localPath = StringUtility.ToLowerSnakeCase(localPath);
			return localPath;
		}
	}
}