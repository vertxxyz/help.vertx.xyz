using Markdig;
using Markdig.Prism;
using Troubleshooter.Constants;

namespace Troubleshooter
{
	public static partial class SiteBuilder
	{
		public static void Build(Arguments arguments)
		{
			var pipeline = new MarkdownPipelineBuilder()
				.UseAdvancedExtensions()
				.UsePrism()
				.Build();

			Site site = new Site(arguments.TroubleshooterRoot);

			using (new BuildScope(arguments))
			{
				BuildPages(arguments, site, pipeline);
				BuildContent(arguments, site);
			}
		}
		
		public static void ContentBuild(Arguments arguments)
		{
			Site site = new Site(arguments.TroubleshooterRoot);

			BuildContent(arguments, site);
		}
	}
}