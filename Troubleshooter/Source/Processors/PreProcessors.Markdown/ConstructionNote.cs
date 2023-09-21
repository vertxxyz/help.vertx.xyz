using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Adds a second new line behind a linebreak if there is not one.
/// </summary>
[UsedImplicitly]
public partial class ConstructionNote : IMarkdownPreProcessor
{
	private static readonly Regex s_regex = GetConstructionNoteRegex();

	public string Process(string text) =>
		s_regex.Replace(text, ":::construction  \n$0  \n:::  ");

	[GeneratedRegex(@"^🚧.+?🚧\r?$", RegexOptions.Compiled | RegexOptions.Multiline)]
	private static partial Regex GetConstructionNoteRegex();
}
