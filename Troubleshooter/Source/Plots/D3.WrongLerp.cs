using OpenQA.Selenium;

namespace Troubleshooter;

public sealed partial class D3
{
	private static string WrongLerpGraph(IJavaScriptExecutor webDriver)
	{
		return "";
		
		// TODO implement diagram.
		/*string svg = (string)webDriver.ExecuteScript(
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

		return svg;*/
	}
}