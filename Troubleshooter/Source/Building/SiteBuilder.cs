using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	public static async Task<(bool success, IEnumerable<string> paths)> Build(
		Arguments arguments,
		Site site,
		MarkdownPipeline pipeline,
		IProcessorGroup processors,
		bool cleanup
	)
	{
		using var buildScope = new BuildScope(arguments, cleanup);
		try
		{
			BuildPages(arguments, site, pipeline, processors);
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
		Site site = new(arguments);
		await BuildContent(arguments, site);
	}
}