using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Troubleshooter;

public interface IHtmlPostProcessor
{
	string Process(string html, string fullPath);
	int Order => 0;
}

/// <summary>
/// Runs on HTML after it has been processed from markdown.
/// </summary>
public sealed class HtmlPostProcessors
{
	private readonly IHtmlPostProcessor[] _all;

	public HtmlPostProcessors(IServiceProvider provider) =>
		_all = typeof(IHtmlPostProcessor).Assembly.GetTypes()
			.Where(t => typeof(IHtmlPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IHtmlPostProcessor)ActivatorUtilities.CreateInstance(provider, t))
			.OrderBy(p => p.Order)
			.ToArray();

	public string Process(string html, string fullPath) => _all.Aggregate(html, (current, processor) => processor.Process(current, fullPath));
}