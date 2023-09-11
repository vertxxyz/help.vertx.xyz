using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Troubleshooter;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

IServiceCollection services = builder.Services;
services.AddSiteConfiguration(args, builder.Configuration["Port"] ?? "3000");
services.AddControllers();
services.AddMemoryCache();
services.AddLogging();

await services.AddMarkdownPipelineAsync();
services.AddProcessors();
var app = builder.Build();

var arguments = app.Services.GetRequiredService<Arguments>();

app.UseRewriter(new RewriteOptions().Add(new RewritePagesTo("/404.html", true)));

app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" }, FileProvider = new PhysicalFileProvider(arguments.Path) });
app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(arguments.Path) });
app.MapControllers();

app.Run(arguments.Host);

// TODO StartAsync doesn't take a URL
// await app.StartAsync();
//
// await ActivatorUtilities.CreateInstance<BuildOnStartup>(app.Services).RebuildIfNotBuiltBefore();
//
// await app.WaitForShutdownAsync();

// static void LogExternalUrls(Arguments arguments) => SiteLogging.LogAllExternalUrls(arguments);