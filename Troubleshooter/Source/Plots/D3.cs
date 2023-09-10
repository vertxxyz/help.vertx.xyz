using System;
using Markdig.Renderers;
using OpenQA.Selenium;

namespace Troubleshooter;

internal sealed partial class D3
{
	public static void Plot(string key, HtmlRenderer renderer)
	{
		WebDriver webDriver = WebRenderer.Driver;
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