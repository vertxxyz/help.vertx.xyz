using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Adds a proper foldable structure to spans containing a Collapsable code segment.
/// </summary>
[UsedImplicitly]
public sealed partial class CollapsableCodeSegmentsReplacement : IHtmlPostProcessor
{
	[GeneratedRegex("""
	                (<span class="token comment">/\* Collapsable: (?<description>[^*]+?) \*/</span>)(\s+)(?<contents>(.|
	                )*?)(\s+)(<span class="token comment">/\* End Collapsable \*/</span>)
	                """)]
	private static partial Regex GetCollapsableCodeSegmentRegex();

	private static readonly Regex s_CollapsableCodeSegmentRegex = GetCollapsableCodeSegmentRegex();

	public string Process(string html, string fullPath)
		=> s_CollapsableCodeSegmentRegex.Replace(html,
			"<span class=\"collapsable collapsable--collapsed\"><svg xmlns=\"http://www.w3.org/2000/svg\" class=\"collapsable__icon\" onclick=\"toggleCollapsedCode(this)\"><use href=\"#code-expand-icon\"></use></svg><a class=\"collapsable__description\" onclick=\"toggleCollapsedCode(this)\">${description}</a><span class=\"collapsable__contents\">${contents}</span></span>"
		);
}