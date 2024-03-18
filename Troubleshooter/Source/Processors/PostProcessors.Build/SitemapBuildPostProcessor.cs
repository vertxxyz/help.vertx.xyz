using System.IO;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Troubleshooter;

/// <summary>
/// Generates the sitemap.txt file
/// </summary>
[UsedImplicitly]
public sealed class SitemapBuildPostProcessor : IBuildPostProcessor
{
	public void Process(Arguments arguments, PageResourcesLookup resources, Site site, ILogger logger)
	{
		// Gather file paths.
		StringBuilder sitemapText = new();
		foreach ((_, PageResource value) in resources)
		{
			if (value.IsSidebar || value.Type != ResourceType.Markdown || value.Location != ResourceLocation.Site)
				continue;

			if (value.SymlinkFullPath != null)
				continue;

			sitemapText.AppendLine($"https://unity.huh.how/{value.OutputLink}");
		}

		IOUtility.CreateFileIfDifferent(Path.Combine(arguments.Path, "sitemap.txt"), sitemapText.ToString(), IOUtility.RecordType.Normal);
	}
}
