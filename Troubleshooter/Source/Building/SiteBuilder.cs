using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Troubleshooter.Constants;
using Troubleshooter.Renderers;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	public static async Task<(bool success, IEnumerable<string> paths)> Build(Arguments arguments, bool cleanup)
	{
		var pipeline = new MarkdownPipelineBuilder()
			.UseAdvancedExtensions()
			.UsePrism()
			// TOC doesn't run properly on the second pass, requires debugging.
			/*.UseTableOfContent(options =>
			{
				options.ContainerTag = "div";
				options.ContainerClass = "table-of-contents";
			})*/
			.Build();

		Site site = new(arguments.TroubleshooterRoot);

		using var buildScope = new BuildScope(arguments, cleanup);
		try
		{
			BuildPages(arguments, site, pipeline);
			await BuildContent(arguments, site);
		}
		catch (BuildException e)
		{
			Console.WriteLine();
			Console.WriteLine(e);
			buildScope.MarkBuildAsFailed();
			return (false, Enumerable.Empty<string>());
		}

		return (true, IOUtility.RecordedPaths);
	}
		
	public static async Task ContentBuild(Arguments arguments)
	{
		Site site = new(arguments.TroubleshooterRoot);
		await BuildContent(arguments, site);
	}
}