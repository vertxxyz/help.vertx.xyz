using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Troubleshooter;
using Troubleshooter.Search;

const string RebuildAllKey = "rebuild-all";
const string RebuildContentKey = "rebuild-content";

// Retrieve arguments.
Arguments arguments = new(args);

try
{
	// Register this for RtfPipe.
	Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	// Load all the online resources before we proceed.
	await OnlineResources.LoadAll();

	await RebuildIfNotBuiltBefore(arguments);

	var builder = WebApplication.CreateBuilder();
	builder.Configuration.AddJsonFile("appsettings.json",
		optional: true,
		reloadOnChange: true);
	string port = builder.Configuration["Port"] ?? "3000";
	var app = builder.Build();

	app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" }, FileProvider = new PhysicalFileProvider(arguments.Path) });
	app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(arguments.Path) });

	app.MapPost("/tools", Tools);

	app.Run($"http://localhost:{port}");
}
catch (Exception e)
{
	LogExitException(e, "Loading online resources failed!");
}

return;

async Task Tools(HttpContext context)
{
	if (context.Request.Form.TryGetValue("rebuild", out var value))
	{
		switch (value[0])
		{
			case RebuildAllKey:
				await Build(arguments);
				break;
			case RebuildContentKey:
				await BuildContent(arguments);
				break;
			default:
				throw new ArgumentException($"{value[0]} not supported by tooling.");
		}
	}

	Console.WriteLine(context);
}

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

static async Task Build(Arguments arguments)
{
	(bool success, IEnumerable<string> paths) = await SiteBuilder.Build(arguments, false);
	if (success)
	{
		Console.WriteLine("Successful build, generating search index.");
		await SearchIndex.Generate(arguments, paths);
		Console.WriteLine("Search index generated.");
	}
	else
	{
		Console.WriteLine("Build failed! Press key to continue.");
		Console.ReadKey();
		Console.Clear();
	}
}

static async Task BuildContent(Arguments arguments)
{
	// Content Build
	await SiteBuilder.ContentBuild(arguments);
	Console.WriteLine("Successful Content Build.");
}

static async Task RebuildIfNotBuiltBefore(Arguments arguments)
{
	string indexPath = Path.Combine(arguments.Path, "index.html");
	if (File.Exists(indexPath))
	{
		if (File.ReadAllText(indexPath).Contains(RebuildAllKey))
			return; // index.html already exists at path, and contains enough to rebuild the app.
	}

	Console.WriteLine("No site present at path. Building for the first time...");
	await Build(arguments);
}

// static void LogExternalUrls(Arguments arguments) => SiteLogging.LogAllExternalUrls(arguments);