using System.IO;
using JetBrains.Annotations;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Troubleshooter.Renderers
{
	public class CustomHtmlRenderer : HtmlRenderer
	{
		public CustomHtmlRenderer([NotNull] TextWriter writer) : base(writer)
		{
			for (int i = 0; i < ObjectRenderers.Count; i++)
			{
				switch (ObjectRenderers[i])
				{
					case HeadingRenderer:
						ObjectRenderers[i] = new HeadingOverrideRenderer();
						break;
					default:
						continue;
				}
			}
		}

		public void DoReset() => Reset();
	}
}