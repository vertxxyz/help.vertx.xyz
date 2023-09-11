using JavaScriptEngineSwitcher.Core;
using JetBrains.Annotations;
using Markdig.Renderers;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Troubleshooter;

[UsedImplicitly]
public sealed class Nomnoml
{
	private readonly IJsEngine _engine;
	private readonly ILogger _logger;

	public Nomnoml(IJsEngine jsEngine, OnlineResources resources, ILogger<Nomnoml> logger)
	{
		_engine = jsEngine;
		_logger = logger;
		_engine.Execute(resources.Graphre);
		_engine.Execute(resources.Nomnoml);
	}
	
	public void Plot(string diagram, HtmlRenderer renderer)
	{
		diagram = $"""
		           #font: "Roboto", sans-serif
		           #fontSize: 11
		           #fill: #282828; #282828
		           #stroke: #D0D0D0
		           #arrowSize: .8
		           #fillArrows: true
		           #lineWidth: 2
		           #gutter: 1
		           #edges: hard
		           {diagram}
		           """;

		// language=javascript
		var js = $"nomnomlSvg = nomnoml.renderSvg(`{diagram.Trim().Replace(@"\", @"\\")}`);";

		_logger.Log(LogLevel.Debug, "\"\"\"\n{Js}\n\"\"\"", js);

		// Inject the diagram into the body of the page.
		_engine.Execute(js);
		string svg = _engine.Evaluate<string>("nomnomlSvg");

		renderer.Write("<div class=\"nomnoml\">").Write(svg).Write("</div>");
	}
}