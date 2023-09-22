using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Markdig;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	/// <summary>
	/// Builds the site and content, and returns a modal result and the written paths.
	/// </summary>
	public static async Task<(bool success, ReadOnlyDictionary<string, IOUtility.RecordType> paths)> Build(
		Arguments arguments,
		Site site,
		MarkdownPipeline pipeline,
		IProcessorGroup processors
	)
	{
		using var buildScope = new BuildScope();
		try
		{
			BuildPages(arguments, site, pipeline, processors);
			await BuildContent(arguments, site);
		}
		catch (BuildException e)
		{
			Console.WriteLine();
			Console.WriteLine(e);
			return (false, new Dictionary<string, IOUtility.RecordType>().AsReadOnly());
		}

		return (true, IOUtility.RecordedPathsLookup);
	}

	/// <summary>
	/// Only builds the Assets/Content directory.
	/// </summary>
	public static async Task ContentBuild(Arguments arguments)
	{
		Site site = new(arguments);
		await BuildContent(arguments, site);
	}
}
