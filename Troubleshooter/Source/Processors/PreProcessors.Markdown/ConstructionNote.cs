using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Adds a second new line behind a linebreak if there is not one.
/// </summary>
[UsedImplicitly]
public partial class ConstructionNote : IMarkdownPreProcessor
{
	[GeneratedRegex(@"^🚧.+?🚧\r?$", RegexOptions.Multiline)]
	private static partial Regex ConstructionNoteRegex { get; }

	public string Process(string text) =>
		ConstructionNoteRegex.Replace(text, ":::construction  \n$0  \n:::  ");

}
