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
	private static readonly Regex s_regex = GetNumberRegex();

	public string Process(string text) => s_regex.Replace(text, "$1");

	[GeneratedRegex(@"(]\(|\/)\d+%20", RegexOptions.Compiled)]
	private static partial Regex GetNumberRegex();
}
