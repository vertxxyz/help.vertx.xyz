// #define FORCE_REBUILD
using System;
#if !FORCE_REBUILD
using System.IO;
#endif
using System.Threading.Tasks;
// using Markdig;
using Troubleshooter.Middleware;

namespace Troubleshooter;

public sealed class BuildOnStartup(Arguments arguments, Site site)
{
	private readonly Site _site = site;
	private readonly string _indexOutputPath = Path.Combine(arguments.Path, "index.html");

	public Task RebuildIfNotBuiltBefore()
	{
		if (!IsBuildRequired())
			return Task.CompletedTask; // index.html already exists at path, and contains enough to rebuild the app.

		return SiteBuilder.ContentBuild(arguments);
	}

	// ReSharper disable once MemberCanBeMadeStatic.Local
	private bool IsBuildRequired()
	{
#if !FORCE_REBUILD
		if (File.Exists(_indexOutputPath))
		{
			if (File.ReadAllText(_indexOutputPath).Contains(BuildSiteController.RebuildAllKey))
				return false;
		}

		Console.WriteLine("No site present at path. Building for the first time...");
#else
		Console.WriteLine("Building (FORCE_REBUILD active)...");
#endif
		return true;
	}
}
