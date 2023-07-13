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
public partial class LineBreakRepair : IMarkdownPreProcessor
{
	private static readonly Regex regex = GetLineBreakRegex();
		
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

    [GeneratedRegex(@"(?<!\r\n)\r\n(---)(?:\s|$)", RegexOptions.Compiled)]
    private static partial Regex GetLineBreakRegex();
}

/// <summary>
/// Numbered pages have their numbers stripped, the numbers are used to order in the authoring folder only.<br/>
/// See the physics messages pages that are numbered eg. "1 2D Physics Messages.md"
/// </summary>
[UsedImplicitly]
public partial class RemoveNumberedPageLinks : IMarkdownPreProcessor
{
	private static readonly Regex regex = GetNumberRegex();
    
	public string Process(string text) => regex.Replace(text, "$1");

	[GeneratedRegex(@"(]\(|\/)\d+%20", RegexOptions.Compiled)]
	private static partial Regex GetNumberRegex();
}