using System;
using System.IO;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static partial class SiteBuilder
	{		
		public static int GetSiteRootIndex(Site site, ResourceLocation resourceLocation)
		{
			string directory;
			switch (resourceLocation)
			{
				case ResourceLocation.Embed:
					directory = site.EmbedsDirectory;
					break;
				case ResourceLocation.Site:
					directory = site.Directory;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(resourceLocation), resourceLocation, null);
			}
			
			string rootDirectory = Path.GetFullPath(directory);
			int siteRootIndex = rootDirectory.Length + 1;
			return siteRootIndex;
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
			return localPath;
		}
	}
}