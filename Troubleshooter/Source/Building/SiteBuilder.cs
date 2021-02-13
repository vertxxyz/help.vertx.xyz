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

			var allPages = BuildPages(arguments, site, pipeline);
			BuildLinksJson(arguments, site, allPages);
			BuildContent(arguments, site);
		}
	}
}