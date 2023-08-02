using System;
using System.Linq;

namespace Troubleshooter;

public interface IMarkdownPreProcessor
{
	string Process(string text);
}

/// <summary>
/// Runs on markdown before it is processed.
/// </summary>
public static class MarkdownPreProcessors
{
	private static readonly IMarkdownPreProcessor[] All =
		typeof(IMarkdownPreProcessor).Assembly.GetTypes()
			.Where(t => typeof(IMarkdownPreProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IMarkdownPreProcessor) Activator.CreateInstance(t)!).ToArray();

	public static string Process(string html) => All.Aggregate(html, (current, processor) => processor.Process(current));
}