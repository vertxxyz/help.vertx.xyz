using System.Web;
using JetBrains.Annotations;
using Markdig.Renderers;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace Troubleshooter;

[UsedImplicitly]
public sealed class Mermaid
{
	private readonly WebRenderer _webRenderer;
	private readonly ILogger _logger;

	public Mermaid(WebRenderer webRenderer, OnlineResources resources, ILogger<Mermaid> logger)
	{
		_webRenderer = webRenderer;
		_logger = logger;
		_webRenderer.Driver.ExecuteScript(resources.Mermaid);
		// language=javascript
		_webRenderer.Driver.ExecuteScript("mermaid.initialize({startOnLoad: false, securityLevel: 'loose'});");
	}

	public void Plot(string diagram, HtmlRenderer renderer)
	{
		WebDriver driver = _webRenderer.Driver;

		// language=javascript
		var js = $$"""
		          var contents = document.querySelector(".contents");
		          contents.innerHTML = `<pre id=\"mermaid\">{{HttpUtility.HtmlEncode(diagram.Trim().Replace(@"\", @"\\"))}}</pre>`;
		          await mermaid.run({ querySelector: "#mermaid" });
		          return contents.querySelector("#mermaid").innerHTML;
		          """;

		_logger.LogDebug("\"\"\"\n{Js}\n\"\"\"", js);

		// Inject the diagram into the body of the page.
		string svg = (string)driver.ExecuteScript(js);

		renderer.Write("<div class=\"mermaid\">").Write(svg).Write("</div>");
	}
}
