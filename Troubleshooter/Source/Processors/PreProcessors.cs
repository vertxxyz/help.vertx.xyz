using System;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

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
	
/// <summary>
/// Adds a second new line behind a linebreak if there is not one.
/// </summary>
[UsedImplicitly]
public class LineBreakRepair : IMarkdownPreProcessor
{
	private static readonly Regex regex = new (@"(?<!\r\n)\r\n(---)(?:\s|$)", RegexOptions.Compiled);
		
	public string Process(string text)
	{
		MatchCollection matchCollection = regex.Matches(text);
		for (var i = 0; i < matchCollection.Count; i++)
		{
			Match match = matchCollection[i];
			Group group = match.Groups[1];
			text = text.Insert(group.Index + i * 2, "\r\n");
		}

		return text;
	}
}