// #define FORCE_REBUILD

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Troubleshooter;
using Troubleshooter.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

IServiceCollection services = builder.Services;
services.AddSiteConfiguration(args, builder.Configuration["Port"] ?? "3000");
services.AddControllers();
services.AddMemoryCache();
services.AddLogging();

try
{
	await services.AddMarkdownPipelineAsync();

	var app = builder.Build();

	// Build the site if we've not already built the site locally.
	await RebuildIfNotBuiltBefore(app.Services);

	var arguments = app.Services.GetRequiredService<Arguments>();

	app.UseRewriter(new RewriteOptions().Add(new RewritePagesTo("/404.html", true)));

	app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" }, FileProvider = new PhysicalFileProvider(arguments.Path) });
	app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(arguments.Path) });
	app.MapControllers();

	// app.MapFallback(@".*(?<!(index\.html)|(404\.html)|(_sidebar\.html))$", rewriteContext =>
	// {
	// 	rewriteContext.Request.Path = "/404.html";
	// 	return Task.CompletedTask;
	// });
	app.Run(arguments.Host);
}
catch (Exception e)
{
	LogExitException(e, "Loading online resources failed!");
}

return;


static async Task RebuildIfNotBuiltBefore(IServiceProvider serviceProvider)
{
	var arguments = serviceProvider.GetRequiredService<Arguments>();

#if !FORCE_REBUILD
	string indexPath = Path.Combine(arguments.Path, "index.html");
	if (File.Exists(indexPath))
	{
		if (File.ReadAllText(indexPath).Contains(BuildSiteController.RebuildAllKey))
			return; // index.html already exists at path, and contains enough to rebuild the app.
	}

	Console.WriteLine("No site present at path. Building for the first time...");
#else
	Console.WriteLine("Building (FORCE_REBUILD active)...");
#endif

	await BuildSiteController.Build(arguments, serviceProvider.GetRequiredService<Site>(), serviceProvider.GetRequiredService<MarkdownPipeline>());
}

// static void LogExternalUrls(Arguments arguments) => SiteLogging.LogAllExternalUrls(arguments);

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