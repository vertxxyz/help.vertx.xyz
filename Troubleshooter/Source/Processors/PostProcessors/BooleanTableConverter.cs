using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public sealed class BooleanTableConverter : IHtmlPostProcessor
{
	public string Process(string html, string fullPath)
	{
		html = html.Replace("<td>Y</td>", "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableYes\"></td>");
		return html.Replace("<td>N</td>", "<td onmouseover=\"highlightTable(this)\" onmouseout=\"unhighlightTable(this)\" class=\"tableNo\"></td>");
	}
}