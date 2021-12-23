using System;
using Markdig;
using Markdig.Prism;
using Troubleshooter.Constants;

namespace Troubleshooter;

public static partial class SiteBuilder
{
	public static bool Build(Arguments arguments)
	{
		var pipeline = new MarkdownPipelineBuilder()
			.UseAdvancedExtensions()
			.UsePrism()
			.Build();

		Site site = new Site(arguments.TroubleshooterRoot);

		using var buildScope = new BuildScope(arguments);
		try
		{
			BuildPages(arguments, site, pipeline);
			BuildContent(arguments, site);
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
		Site site = new Site(arguments.TroubleshooterRoot);
		BuildContent(arguments, site);
	}
}