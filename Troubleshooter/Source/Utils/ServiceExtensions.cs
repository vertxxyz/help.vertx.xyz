using System.Text;
using System.Threading.Tasks;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using Troubleshooter.Renderers;

namespace Troubleshooter;

public static class ServiceExtensions
{
	// ReSharper disable once UnusedMethodReturnValue.Global
	public static IServiceCollection AddSiteConfiguration(this IServiceCollection services, string[] args, string port)
	{
		Arguments arguments = new(args) { Host = $"http://localhost:{port}" };
		services.AddSingleton(arguments);
		services.AddSingleton<IRootPathProvider>(arguments);
		services.AddSingleton<WebRenderer>();
		services.AddSingleton<IJsEngine, V8JsEngine>();
		services.AddSingleton<Site>();
		return services;
	}
	
	// ReSharper disable once UnusedMethodReturnValue.Global
	public static async Task<IServiceCollection> AddMarkdownPipelineAsync(this IServiceCollection services)
	{
		// Register this for RtfPipe.
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		// Load all the online resources before we proceed.
		OnlineResources onlineResources = new OnlineResources();
		await onlineResources.LoadAll();
		services.AddSingleton(onlineResources);
		services.AddSingleton<MarkdownPipeline>(
			provider => new MarkdownPipelineBuilder()
				.UseAdvancedExtensions()
				.UseCodeHighlighting(provider)
				// TOC doesn't run properly on the second pass, requires debugging.
				/*.UseTableOfContent(options =>
					{
						options.ContainerTag = "div";
						options.ContainerClass = "table-of-contents";
					})*/
				.Build()
		);
		
		return services;
	}
}