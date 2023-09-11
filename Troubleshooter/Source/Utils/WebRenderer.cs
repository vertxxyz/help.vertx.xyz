using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Troubleshooter;

public class WebRenderer
{
	public Arguments Arguments { get; }
	public WebDriver Driver { get; }

	public WebRenderer(Arguments arguments)
	{
		var options = new EdgeOptions();
		// options.AddArgument("--headless");
		options.AddArguments("--disable-extensions", "--window-size=1920,1080");
		Driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), options);
		Arguments = arguments;
		
		Driver.Url = arguments.Host;
		
		AppDomain.CurrentDomain.ProcessExit += (_, _) => Driver.Quit();
	}
}