using System;
using System.IO;
using System.Text;
using Markdig;
using Markdig.Helpers;

namespace Troubleshooter.Renderers;

public static class CustomPipelineExtensions
{
	private static HtmlRendererCache? s_rendererCache;

	internal static CustomHtmlRendererScope RentCustomHtmlRenderer(this MarkdownPipeline pipeline)
	{
		var cache = s_rendererCache ??= new HtmlRendererCache(pipeline);
		CustomHtmlRenderer renderer = cache.Get();
		return new CustomHtmlRendererScope(cache, renderer);
	}

	internal sealed class HtmlRendererCache(MarkdownPipeline pipeline) : ObjectCache<CustomHtmlRenderer>
	{
		private const int InitialCapacity = 1024;

		private readonly MarkdownPipeline _pipeline = pipeline;

		protected override CustomHtmlRenderer NewInstance()
		{
			var bodyWriter = new StringWriter(new StringBuilder(InitialCapacity)) { NewLine = "\n" };
			var headWriter = new StringWriter(new StringBuilder(InitialCapacity)) { NewLine = "\n" };
			var renderer = new CustomHtmlRenderer(headWriter, bodyWriter);
			_pipeline.Setup(renderer);
			renderer.Setup();
			return renderer;
		}

		protected override void Reset(CustomHtmlRenderer instance) => instance.DoReset();
	}

	internal readonly struct CustomHtmlRendererScope : IDisposable
	{
		private readonly HtmlRendererCache _cache;
		public readonly CustomHtmlRenderer Instance;

		internal CustomHtmlRendererScope(HtmlRendererCache cache, CustomHtmlRenderer renderer)
		{
			_cache = cache;
			Instance = renderer;
		}

		public void Dispose() => _cache.Release(Instance);
	}
}
