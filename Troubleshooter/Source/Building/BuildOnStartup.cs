// #define FORCE_REBUILD
using System;
#if !FORCE_REBUILD
using System.IO;
#endif
using System.Threading.Tasks;
using Markdig;
using Troubleshooter.Middleware;

namespace Troubleshooter;

public sealed class BuildOnStartup
{
	private readonly Arguments _arguments;
	private readonly Site _site;
	private readonly MarkdownPipeline _pipeline;

	public BuildOnStartup(Arguments arguments, Site site, MarkdownPipeline pipeline)
	{
		_arguments = arguments;
		_site = site;
		_pipeline = pipeline;
	}
	
	public async Task RebuildIfNotBuiltBefore()
	{
		if (!IsBuildRequired())
			return; // index.html already exists at path, and contains enough to rebuild the app.

		var host = _arguments.Host;
		// TODO can't make this build from localhost yet. Presumably the app hasn't fully started yet when this runs?
		// You can build just fine from the POST request.
		_arguments.OverrideHost("https://unity.huh.how/");
		
		await BuildSiteController.Build(_arguments, _site, _pipeline);
		_arguments.OverrideHost(host);
	}

	// ReSharper disable once MemberCanBeMadeStatic.Local
	private bool IsBuildRequired()
	{
#if !FORCE_REBUILD
		string indexPath = Path.Combine(_arguments.Path, "index.html");
		if (File.Exists(indexPath))
		{
			if (File.ReadAllText(indexPath).Contains(BuildSiteController.RebuildAllKey))
				return false;
		}

		Console.WriteLine("No site present at path. Building for the first time...");
#else
		Console.WriteLine("Building (FORCE_REBUILD active)...");
#endif
		return true;
	}
}