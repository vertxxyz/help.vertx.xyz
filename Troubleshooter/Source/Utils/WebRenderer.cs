using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Troubleshooter;

public class WebRenderer : IHostedService
{
	public Arguments Arguments { get; }
	public WebDriver Driver { get; }

	public WebRenderer(Arguments arguments)
	{
		var options = new EdgeOptions();
		// options.AddArgument("headless");
		options.AddArgument("disable-extensions");
		Driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), options);
		Arguments = arguments;
	}


	public Task StartAsync(CancellationToken cancellationToken)
	{
		// AppDomain.CurrentDomain.ProcessExit += (_, _) => { Driver.Quit(); };
		
		// Set the webdriver (the browser used to render diagrams) to point to our running host.
		// Driver.Url = Arguments.Host;
		Driver.Url = "https://unity.huh.how/";
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		Driver.Quit();
		return Task.CompletedTask;
	}
}