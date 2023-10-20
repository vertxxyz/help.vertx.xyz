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
		var onlineResources = new OnlineResources();
		await onlineResources.LoadAll();
		services.AddSingleton(onlineResources);
		services.AddSingleton<MarkdownPipeline>(
			provider => new MarkdownPipelineBuilder()
				.UseAdvancedExtensions()
				.UseYamlFrontMatter()
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

	/// <summary>
	/// Adds pre and post processors for the Markdown to HTML pipeline.
	/// </summary>
	// ReSharper disable once UnusedMethodReturnValue.Global
	public static IServiceCollection AddProcessors(this IServiceCollection services)
	{
		services.AddSingleton(provider => new MarkdownPreProcessors(provider));
		services.AddSingleton(provider => new HtmlPostProcessors(provider));
		services.AddSingleton(provider => new PageResourcesPostProcessors(provider));
		services.AddSingleton(provider => new BuildPostProcessors(provider));
		services.AddSingleton<IProcessorGroup, ProcessorsGroup>();
		return services;
	}
}
