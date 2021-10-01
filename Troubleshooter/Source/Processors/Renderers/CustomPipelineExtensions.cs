using System;
using System.IO;
using System.Text;
using Markdig;
using Markdig.Helpers;

namespace Troubleshooter.Renderers
{
	public static class CustomPipelineExtensions
	{
		private static HtmlRendererCache _rendererCache;
		private static HtmlRendererCache _rendererCacheForCustomWriter;

		internal static RentedCustomHtmlRenderer RentCustomHtmlRenderer(this MarkdownPipeline pipeline)
		{
			HtmlRendererCache cache = _rendererCache ??= new HtmlRendererCache(pipeline);
			CustomHtmlRenderer renderer = cache.Get();
			return new RentedCustomHtmlRenderer(cache, renderer);
		}

		internal sealed class HtmlRendererCache : ObjectCache<CustomHtmlRenderer>
		{
			private const int InitialCapacity = 1024;

			private readonly MarkdownPipeline _pipeline;

			public HtmlRendererCache(MarkdownPipeline pipeline)
			{
				_pipeline = pipeline;
			}

			protected override CustomHtmlRenderer NewInstance()
			{
				var writer = new StringWriter(new StringBuilder(InitialCapacity));
				var renderer = new CustomHtmlRenderer(writer);
				_pipeline.Setup(renderer);
				return renderer;
			}

			protected override void Reset(CustomHtmlRenderer instance)
			{
				instance.DoReset();
				((StringWriter)instance.Writer).GetStringBuilder().Length = 0;
			}
		}

		internal readonly struct RentedCustomHtmlRenderer : IDisposable
		{
			private readonly HtmlRendererCache _cache;
			public readonly CustomHtmlRenderer Instance;

			internal RentedCustomHtmlRenderer(HtmlRendererCache cache, CustomHtmlRenderer renderer)
			{
				_cache = cache;
				Instance = renderer;
			}

			public void Dispose() => _cache.Release(Instance);
		}
	}
}