using System;
using System.Threading.Tasks;
using Markdig;
using Markdig.Prism;
using Troubleshooter.Constants;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	public static async Task<bool> Build(Arguments arguments)
	{
		var pipeline = new MarkdownPipelineBuilder()
			.UseAdvancedExtensions()
			.UsePrism()
			.Build();

		Site site = new(arguments.TroubleshooterRoot);

		using var buildScope = new BuildScope(arguments);
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
			return false;
		}

		return true;
	}
		
	public static void ContentBuild(Arguments arguments)
	{
		Site site = new(arguments.TroubleshooterRoot);
		BuildContent(arguments, site);
	}
}