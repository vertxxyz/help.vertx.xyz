using System;
using JetBrains.Annotations;
using Markdig.Renderers;
using OpenQA.Selenium;

namespace Troubleshooter;

[UsedImplicitly]
public sealed partial class D3
{
	private readonly WebRenderer _webRenderer;

	public D3(WebRenderer webRenderer, OnlineResources resources)
	{
		_webRenderer = webRenderer;
		WebDriver webDriver = _webRenderer.Driver;
		webDriver.ExecuteScript(resources.D3);
		webDriver.ExecuteScript(resources.Plot);
	}
	
	public void Plot(string key, HtmlRenderer renderer)
	{
		WebDriver webDriver = _webRenderer.Driver;
		string svg = key switch
		{
			"graph-wrong-lerp" => WrongLerpGraph(webDriver),
			_ => throw new ArgumentOutOfRangeException(key, $"{key} is not yet supported by {nameof(D3)}.{nameof(Plot)}.")
		};

		renderer.Write("<div class=\"d3\">").Write(svg).Write("</code>").Write("</div>");
	}
}