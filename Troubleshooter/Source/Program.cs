using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Troubleshooter;
using Troubleshooter.Middleware;
using Troubleshooter.Renderers;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
string port = builder.Configuration["Port"] ?? "3000";

// Retrieve arguments.
Arguments arguments = new(args) { Host = $"http://localhost:{port}" };

IServiceCollection services = builder.Services;
services.AddSingleton<Arguments>(_ => arguments);
services.AddSingleton<IRootPathProvider, Arguments>(_ => arguments);
services.AddHostedService<WebRenderer>();
services.AddSingleton<Site>();
services.AddControllers();

try
{
	// Register this for RtfPipe.
	Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	// Load all the online resources before we proceed.

	OnlineResources onlineResources = new OnlineResources();
	await onlineResources.LoadAll();
	services.AddSingleton<OnlineResources>(_ => onlineResources);
	
	services.AddSingleton<MarkdownPipeline>(provider =>
	{
		var webRenderer = provider.GetRequiredService<WebRenderer>();
		return new MarkdownPipelineBuilder()
			.UseAdvancedExtensions()
			.UseCodeHighlighting(webRenderer)
			// TOC doesn't run properly on the second pass, requires debugging.
			/*.UseTableOfContent(options =>
			{
				options.ContainerTag = "div";
				options.ContainerClass = "table-of-contents";
			})*/
			.Build();
	});
	
	var app = builder.Build();

	// Build the site if we've not already built the site locally.
	await RebuildIfNotBuiltBefore(app.Services);

	app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" }, FileProvider = new PhysicalFileProvider(arguments.Path) });
	app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(arguments.Path) });

	app.MapControllers();
	
	app.Run(arguments.Host);
}
catch (Exception e)
{
	LogExitException(e, "Loading online resources failed!");
}

return;


static void LogExitException(Exception e, string message)
{
	Console.WriteLine("----------------");
	Console.WriteLine(message);
	Console.WriteLine("----------------");
	Console.WriteLine(e.Message);
	Console.WriteLine();
	Console.WriteLine(e.StackTrace);
	Console.WriteLine("----------------");

	Console.WriteLine("Press any key to exit.");
	Console.ReadKey();
}

static async Task RebuildIfNotBuiltBefore(IServiceProvider serviceProvider)
{
	var arguments = serviceProvider.GetRequiredService<Arguments>();

	string indexPath = Path.Combine(arguments.Path, "index.html");
	if (File.Exists(indexPath))
	{
		if (File.ReadAllText(indexPath).Contains(BuildSiteController.RebuildAllKey))
			return; // index.html already exists at path, and contains enough to rebuild the app.
	}

	Console.WriteLine("No site present at path. Building for the first time...");
	await BuildSiteController.Build(arguments, serviceProvider.GetRequiredService<Site>(), serviceProvider.GetRequiredService<MarkdownPipeline>());
}

// static void LogExternalUrls(Arguments arguments) => SiteLogging.LogAllExternalUrls(arguments);