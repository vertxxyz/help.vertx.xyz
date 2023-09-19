using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Troubleshooter;

/// <summary>
/// Post processors for page resources (.md, .html, etc).
/// </summary>
public interface IBuildPostProcessor
{
	void Process(Arguments arguments, PageResourcesLookup resources, Site site);
}

/// <summary>
/// Runs on all resources after they have been built.
/// </summary>
public sealed class BuildPostProcessors
{
	private readonly IBuildPostProcessor[] _all;
	public BuildPostProcessors(IServiceProvider provider) =>
		_all = typeof(IBuildPostProcessor).Assembly.GetTypes()
			.Where(t => typeof(IBuildPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IBuildPostProcessor)ActivatorUtilities.CreateInstance(provider, t))
			.ToArray();

	public void Process(Arguments arguments, PageResourcesLookup resources, Site site)
	{
		foreach (IBuildPostProcessor postProcessor in _all)
			postProcessor.Process(arguments, resources, site);
	}
}