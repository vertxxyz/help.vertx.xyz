using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Troubleshooter;

public static class WebRenderer
{
	public static readonly WebDriver Driver;

	static WebRenderer()
	{
		var options = new EdgeOptions();
		// options.AddArgument("headless");
		Driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), options);
		AppDomain.CurrentDomain.ProcessExit += (_, _) =>
		{
			Driver.Quit();
		};
		
		
	}
}