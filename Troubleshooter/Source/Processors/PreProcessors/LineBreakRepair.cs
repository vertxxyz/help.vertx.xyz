using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

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