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
		private static string GetJsonDestination(Arguments arguments) => Path.Combine(arguments.JsonOutputDirectory, Site.LinksJsonFileName);

		private static void BuildLinksJson(Arguments arguments, Site site, Dictionary<string, PageResource> allResources)
		{
			int siteRootIndex = GetSiteRootIndex(site);

			Dictionary<string, string> linksToGuids = allResources
				.Where(kvp => kvp.Value.Location == ResourceLocation.Site)
				.ToDictionary(pair => ConvertFullPathToLinkPath(pair.Key, siteRootIndex), pair => pair.Value.Guid);
			linksToGuids.Add("404", "404");

			var links = new Links(linksToGuids);
			string linksJson = JsonConvert.SerializeObject(links);
			string jsonDestination = GetJsonDestination(arguments);
			if (!File.Exists(jsonDestination) || !string.Equals(File.ReadAllText(jsonDestination), linksJson, StringComparison.Ordinal))
				File.WriteAllText(jsonDestination, linksJson);
		}

		public static int GetSiteRootIndex(Site site)
		{
			string rootDirectory = Path.GetFullPath(site.Directory);
			int siteRootIndex = rootDirectory.Length + 1;
			return siteRootIndex;
		}
		
		public static string ConvertFullPathToLinkPath(string fullPath, int siteRootIndex)
		{
			string localPath = fullPath[siteRootIndex..];
			localPath = localPath.Replace(' ', '-');
			localPath = localPath.Replace("&", "and");
			localPath = localPath.Replace('\\', '/');
			localPath = Path.ChangeExtension(localPath, null);
			return localPath;
		}

		private static Links GetBuiltLinks(Arguments arguments)
		{
			string destination = GetJsonDestination(arguments);
			return File.Exists(destination) ? JsonConvert.DeserializeObject<Links>(File.ReadAllText(destination)) : null;
		}
	}
}