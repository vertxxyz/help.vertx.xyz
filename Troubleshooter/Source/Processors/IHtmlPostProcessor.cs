using System;
using System.Linq;

namespace Troubleshooter;

public interface IHtmlPostProcessor
{
	string Process(string html, string fullPath);
	int Order => 0;
}

/// <summary>
/// Runs on HTML after it has been processed from markdown.
/// </summary>
public static class HtmlPostProcessors
{
	private static readonly IHtmlPostProcessor[] All =
		typeof(IHtmlPostProcessor).Assembly.GetTypes()
			.Where(t => typeof(IHtmlPostProcessor).IsAssignableFrom(t) && !t.IsAbstract)
			.Select(t => (IHtmlPostProcessor)Activator.CreateInstance(t)!)
			.OrderBy(p => p.Order)
			.ToArray();

	public static string Process(string html, string fullPath) => All.Aggregate(html, (current, processor) => processor.Process(current, fullPath));
}