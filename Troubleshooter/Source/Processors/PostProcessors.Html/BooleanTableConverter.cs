using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Adds colors and tooltips to table cells containing Y/N/M characters.
/// </summary>
[UsedImplicitly]
public sealed partial class BooleanTableConverter : IHtmlPostProcessor
{
	public string Process(string html, string fullPath)
	{
		html = YesTdRegex.Replace(html, "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableYes\" title=\"Yes\">$1</td>");
		html = MaybeTdRegex.Replace(html, "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableMaybe\" title=\"Maybe\">$1</td>");
		return NoTdRegex.Replace(html, "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableNo\" title=\"No\">$1</td>");
	}

	[GeneratedRegex("<td>Y( +.+)??</td>")]
	private static partial Regex YesTdRegex { get; }

	[GeneratedRegex("<td>M( +.+)??</td>")]
	private static partial Regex MaybeTdRegex { get; }

	[GeneratedRegex("<td>N( +.+)??</td>")]
	private static partial Regex NoTdRegex { get; }
}
