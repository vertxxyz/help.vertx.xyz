using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Troubleshooter.Search;

namespace Troubleshooter.Middleware;

/// <summary>
/// Responds /tools/{id} POST requests. The top level action that builds the site.
/// </summary>
public sealed class BuildSiteController : ControllerBase
{
	public const string RebuildAllKey = "rebuild-all";
	const string RebuildContentKey = "rebuild-content";

	private readonly Arguments _arguments;
	private readonly Site _site;
	private readonly MarkdownPipeline _markdownPipeline;
	private readonly IProcessorGroup _processors;
	private readonly ILogger _logger;

	public BuildSiteController(
		Arguments arguments,
		Site site,
		MarkdownPipeline markdownPipeline,
		IProcessorGroup processors,
		ILogger logger
	)
	{
		_arguments = arguments;
		_site = site;
		_markdownPipeline = markdownPipeline;
		_processors = processors;
		_logger = logger;
	}

	[HttpPost("/tools/{id}")]
	public async Task<IActionResult> RunTools(string id)
	{
		switch (id)
		{
			case RebuildAllKey:
				await Build(_arguments, _site, _markdownPipeline, _processors);
				break;
			case RebuildContentKey:
				await BuildContent(_arguments);
				break;
			default:
				throw new ArgumentException($"{id} not supported by tooling.");
		}

		return Ok();
	}


	public static async Task Build(
		Arguments arguments,
		Site site,
		MarkdownPipeline markdownPipeline,
		IProcessorGroup processors
	)
	{
		(bool success, ReadOnlyDictionary<string, IOUtility.RecordType> paths) = await SiteBuilder.Build(arguments, site, markdownPipeline, processors);
		if (success)
		{
			Console.WriteLine("Successful build.");
			Console.WriteLine("Generating search index...");
			success = await SearchIndex.Generate(arguments, paths);
			if (success)
			{
				Console.WriteLine("Search index generated.");
				Console.WriteLine("Complete.");
				return;
			}
		}
		Console.WriteLine("Build failed! Press key to continue.");
		Console.ReadKey();
		Console.Clear();
	}

	private async Task BuildContent(Arguments arguments)
	{
		// Content Build
		await SiteBuilder.ContentBuild(arguments);
		Console.WriteLine("Successful Content Build.");
	}
}
