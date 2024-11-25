using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Numbered pages have their numbers stripped, the numbers are used to order in the authoring folder only.<br/>
/// See the physics messages pages that are numbered eg. "1 2D Physics Messages.md"
/// </summary>
[UsedImplicitly]
public partial class RemoveNumberedPageLinks : IMarkdownPreProcessor
{
	[GeneratedRegex(@"(]\(|\/)\d+%20")]
	private static partial Regex NumberRegex { get; }

	public string Process(string text) => NumberRegex.Replace(text, "$1");
}
