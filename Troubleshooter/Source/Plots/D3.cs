using System;
using Markdig.Renderers;
using OpenQA.Selenium;

namespace Troubleshooter;

internal sealed partial class D3
{
	private readonly WebRenderer _webRenderer;

	public D3(WebRenderer webRenderer)
	{
		_webRenderer = webRenderer;
	}
	
	public void Plot(string key, HtmlRenderer renderer)
	{
		WebDriver webDriver = _webRenderer.Driver;
		switch (key)
		{
			case "graph-wrong-lerp":
				WrongLerpGraph(renderer, webDriver);
				break;
			default:
				throw new ArgumentOutOfRangeException(key, $"{key} is not yet supported by {nameof(D3)}.{nameof(Plot)}.");
		}
	}
}