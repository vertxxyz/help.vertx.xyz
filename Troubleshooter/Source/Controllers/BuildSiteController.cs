using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Troubleshooter.Search;

namespace Troubleshooter.Middleware;

public sealed class BuildSiteController : ControllerBase
{
	public const string RebuildAllKey = "rebuild-all";
	const string RebuildContentKey = "rebuild-content";
	
	private readonly Arguments _arguments;
	private readonly Site _site;
	private readonly MarkdownPipeline _markdownPipeline;
	private readonly HtmlPostProcessors _postProcessors;
	private readonly MarkdownPreProcessors _preProcessors;

	public BuildSiteController(Arguments arguments, Site site, MarkdownPipeline markdownPipeline, MarkdownPreProcessors preProcessors, HtmlPostProcessors postProcessors)
	{
		_arguments = arguments;
		_site = site;
		_markdownPipeline = markdownPipeline;
		_preProcessors = preProcessors;
		_postProcessors = postProcessors;
	}

	[HttpPost("/tools/{id}")]
	public async Task<IActionResult> RunTools(string id)
	{
		switch (id)
		{
			case RebuildAllKey:
				await Build(_arguments, _site, _markdownPipeline, _preProcessors, _postProcessors);
				break;
			case RebuildContentKey:
				await BuildContent(_arguments);
				break;
			default:
				throw new ArgumentException($"{id} not supported by tooling.");
		}
		
		return Ok();
	}


	public static async Task Build(Arguments arguments, Site site, MarkdownPipeline markdownPipeline, MarkdownPreProcessors preProcessors, HtmlPostProcessors postProcessors)
	{
		(bool success, IEnumerable<string> paths) = await SiteBuilder.Build(arguments, site, markdownPipeline, preProcessors, postProcessors, false);
		if (success)
		{
			Console.WriteLine("Successful build, generating search index.");
			await SearchIndex.Generate(arguments, paths);
			Console.WriteLine("Search index generated.");
		}
		else
		{
			Console.WriteLine("Build failed! Press key to continue.");
			Console.ReadKey();
			Console.Clear();
		}
	}

	private async Task BuildContent(Arguments arguments)
	{
		// Content Build
		await SiteBuilder.ContentBuild(arguments);
		Console.WriteLine("Successful Content Build.");
	}
}
