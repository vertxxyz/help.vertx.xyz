using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Adds a "has-foo" class to figure tags that contain a div with a class.<br/>
/// This can be used instead of :has() as some browsers still do not support it.
/// </summary>
[UsedImplicitly]
public sealed partial class FigureHasDecorator : IHtmlPostProcessor
{
	[GeneratedRegex("""<figure>(\s*<div class="([\w-]+?)")""")]
	private static partial Regex GetFigureHasRegex();

	private static readonly Regex s_FigureHasRegex = GetFigureHasRegex();
	
	public string Process(string html, string fullPath)
	{
		html = s_FigureHasRegex.Replace(html, "<figure class=\"has-$2\">$1");
		return html;
	}
}