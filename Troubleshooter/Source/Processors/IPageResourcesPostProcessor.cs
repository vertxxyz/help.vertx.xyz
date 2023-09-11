using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

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
public sealed class PageResourcesPostProcessors
{
	private readonly IPageResourcesPostProcessor[] _all;

	public PageResourcesPostProcessors(IServiceProvider provider) =>
		_all = typeof(IPageResourcesPostProcessor).Assembly.GetTypes()
			.Where(t => typeof(IPageResourcesPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IPageResourcesPostProcessor)ActivatorUtilities.CreateInstance(provider, t))
			.ToArray();

	public void Process(PageResourcesLookup pageResources, Site site)
	{
		foreach (IPageResourcesPostProcessor postProcessor in _all)
			postProcessor.Process(pageResources, site);
	}
}