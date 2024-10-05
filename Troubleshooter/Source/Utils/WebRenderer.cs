using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Troubleshooter;

public sealed class WebRenderer
{
	public WebDriver Driver { get; }

	public WebRenderer(Arguments arguments)
	{
		var options = new EdgeOptions();
		options.AddArguments("headless", "disable-extensions", "window-size=2560,1440");
		Driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), options);

		Driver.Url = arguments.Host;

		AppDomain.CurrentDomain.ProcessExit += (_, _) => Driver.Quit();
	}
}
