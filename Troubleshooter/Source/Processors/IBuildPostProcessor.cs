using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Troubleshooter;

/// <summary>
/// Post processors for page resources (.md, .html, etc).
/// </summary>
public interface IBuildPostProcessor
{
	void Process(Arguments arguments, PageResourcesLookup resources, Site site, ILogger logger);

	int Order => 0;
}

/// <summary>
/// Runs on all resources after they have been built.
/// </summary>
public sealed class BuildPostProcessors
{
	private readonly IBuildPostProcessor[] _all;
	private readonly ILogger<BuildPostProcessors> _logger;
	public BuildPostProcessors(IServiceProvider provider)
	{
		_all = typeof(IBuildPostProcessor).Assembly.GetTypes()
			.Where(t => typeof(IBuildPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IBuildPostProcessor)ActivatorUtilities.CreateInstance(provider, t))
			.OrderBy(t => t.Order)
			.ToArray();
		_logger = provider.GetRequiredService<ILogger<BuildPostProcessors>>();
	}

	public void Process(Arguments arguments, PageResourcesLookup resources, Site site)
	{
		foreach (IBuildPostProcessor postProcessor in _all)
			postProcessor.Process(arguments, resources, site, _logger);
	}
}
