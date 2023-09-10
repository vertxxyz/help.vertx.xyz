using System;
using System.Linq;

namespace Troubleshooter;

/// <summary>
/// Post processors for page resources (.md, .html, etc).
/// </summary>
public interface IPageResourcesPostProcessor
{
	void Process(PageResourcesLookup dictionary, Site site);
}

/// <summary>
/// Runs on a PageResource after it has been processed from markdown.
/// </summary>
public static class PageResourcesPostProcessors
{
	private static readonly IPageResourcesPostProcessor[] All =
		typeof(IPageResourcesPostProcessor).Assembly.GetTypes()
			.Where(t => typeof(IPageResourcesPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IPageResourcesPostProcessor)Activator.CreateInstance(t)!).ToArray();

	public static void Process(PageResourcesLookup pageResources, Site site)
	{
		foreach (IPageResourcesPostProcessor postProcessor in All)
			postProcessor.Process(pageResources, site);
	}
}