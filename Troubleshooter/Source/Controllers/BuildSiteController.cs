using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Http;
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

	public BuildSiteController(Arguments arguments, Site site, MarkdownPipeline markdownPipeline)
	{
		_arguments = arguments;
		_site = site;
		_markdownPipeline = markdownPipeline;
	}

	[HttpPost("/tools")]
	public async Task<IActionResult> RunTools(HttpContext context)
	{
		if (context.Request.Form.TryGetValue("rebuild", out var value))
		{
			switch (value[0])
			{
				case RebuildAllKey:
					await Build(_arguments, _site, _markdownPipeline);
					break;
				case RebuildContentKey:
					await BuildContent(_arguments);
					break;
				default:
					throw new ArgumentException($"{value[0]} not supported by tooling.");
			}
		}
		
		return Ok();
	}


	public static async Task Build(Arguments arguments, Site site, MarkdownPipeline markdownPipeline)
	{
		(bool success, IEnumerable<string> paths) = await SiteBuilder.Build(arguments, site, markdownPipeline, false);
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
