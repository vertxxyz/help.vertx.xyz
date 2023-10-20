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
	private readonly IProcessorGroup _processors;
	private readonly string _indexOutputPath;

	public BuildOnStartup(Arguments arguments, Site site, MarkdownPipeline pipeline, IProcessorGroup processors)
	{
		_arguments = arguments;
		_site = site;
		_pipeline = pipeline;
		_processors = processors;
		_indexOutputPath = Path.Combine(_arguments.Path, "index.html");
	}

	private async Task WriteTemporaryIndex()
	{
		string indexOriginPath = Path.Combine(_site.ContentDirectory, "index.html");
		string text = await File.ReadAllTextAsync(indexOriginPath);
		const string replace = "<!-- page content -->";
		text = text.Replace(replace, "Page must be manually rebuilt.");
		await File.WriteAllTextAsync(_indexOutputPath, text);
	}

	public Task RebuildIfNotBuiltBefore()
	{
		if (!IsBuildRequired())
			return Task.CompletedTask; // index.html already exists at path, and contains enough to rebuild the app.

		return WriteTemporaryIndex();

		/*var host = _arguments.Host;
		// TODO can't make this build from localhost yet. Presumably the app hasn't fully started yet when this runs?
		// You can build just fine from the POST request.
		_arguments.OverrideHost("https://unity.huh.how/");

		await BuildSiteController.Build(_arguments, _site, _pipeline, _processors);
		_arguments.OverrideHost(host);*/
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
