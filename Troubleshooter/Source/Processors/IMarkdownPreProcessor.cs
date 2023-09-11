using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Troubleshooter;

public interface IMarkdownPreProcessor
{
	string Process(string text);
}

/// <summary>
/// Runs on markdown before it is processed.
/// </summary>
public sealed class MarkdownPreProcessors
{
	private readonly IMarkdownPreProcessor[] _all;

	public MarkdownPreProcessors(IServiceProvider provider) =>
		_all = typeof(IMarkdownPreProcessor).Assembly.GetTypes()
			.Where(t => typeof(IMarkdownPreProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IMarkdownPreProcessor) ActivatorUtilities.CreateInstance(provider, t))
			.ToArray();

	public string Process(string html) => _all.Aggregate(html, (current, processor) => processor.Process(current));
}