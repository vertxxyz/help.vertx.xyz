using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public sealed partial class BooleanTableConverter : IHtmlPostProcessor
{
	public string Process(string html, string fullPath)
	{
		html = _yesTdRegex.Replace(html, "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableYes\" title=\"Yes\">$1</td>");
		html = _maybeTdRegex.Replace(html, "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableMaybe\" title=\"Maybe\">$1</td>");
		return _noTdRegex.Replace(html, "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableNo\" title=\"No\">$1</td>");
	}

	private readonly Regex _yesTdRegex = GetYesTdRegex();
	private readonly Regex _maybeTdRegex = GetMaybeTdRegex();
	private readonly Regex _noTdRegex = GetNoTdRegex();
	
	[GeneratedRegex("<td>Y( +.+)??</td>")]
	private static partial Regex GetYesTdRegex();
	
	[GeneratedRegex("<td>M( +.+)??</td>")]
	private static partial Regex GetMaybeTdRegex();

    [GeneratedRegex("<td>N( +.+)??</td>")]
    private static partial Regex GetNoTdRegex();
}