using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static partial class SiteBuilder
	{
		private static void BuildLinksJson(Arguments arguments, Site site, Dictionary<string, PageResource> allResources)
		{
			string rootDirectory = Path.GetFullPath(site.Directory);
			int siteRootIndex = rootDirectory.Length + 1;

			Dictionary<string, string> linksToGuids = allResources
				.Where(kvp => kvp.Value.Location == ResourceLocation.Site)
				.ToDictionary(pair => ConvertFullPathToLinkPath(pair.Key), pair => pair.Value.Guid);
			linksToGuids.Add("404", "404");

			var links = new Links(linksToGuids);
			string linksJson = JsonConvert.SerializeObject(links);
			string jsonDestination = Path.Combine(arguments.JsonOutputDirectory, Site.LinksJsonFileName);
			if (!File.Exists(jsonDestination) || !string.Equals(File.ReadAllText(jsonDestination), linksJson, StringComparison.Ordinal))
				File.WriteAllText(jsonDestination, linksJson);

			string ConvertFullPathToLinkPath(string fullPath)
			{
				string localPath = fullPath[siteRootIndex..];
				localPath = localPath.Replace(' ', '-');
				localPath = localPath.Replace("&", "and");
				localPath = localPath.Replace('\\', '/');
				localPath = Path.ChangeExtension(localPath, null);
				return localPath;
			}
		}
	}
}