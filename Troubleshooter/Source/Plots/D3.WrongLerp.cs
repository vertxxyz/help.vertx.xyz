using Markdig.Renderers;
using OpenQA.Selenium;

namespace Troubleshooter;

internal sealed partial class D3
{
	private static void WrongLerpGraph(HtmlRenderer renderer, WebDriver webDriver)
	{
		webDriver.ExecuteScript(OnlineResources.D3);
		webDriver.ExecuteScript(OnlineResources.Plot);
		string svg = (string)webDriver.ExecuteScript(
			// language=javascript
			"""
			return Plot.plot({
				marks: [
					Plot.frame(),
					Plot.text(["Hello, world!"], {frameAnchor: "middle"})
				],
				document: document
			}).outerHTML;
			"""
		);

		renderer.Write("<div class=\"d3\">").Write(svg).Write("</code>").Write("</div>");
	}
}