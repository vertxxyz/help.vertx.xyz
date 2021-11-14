using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using Markdig.Prism;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Troubleshooter.Renderers;

public class CustomHtmlRenderer : HtmlRenderer
{
	public CustomHtmlRenderer([NotNull] TextWriter writer) : base(writer) { }

	public void Setup()
	{
		for (int i = 0; i < ObjectRenderers.Count; i++)
		{
			switch (ObjectRenderers[i])
			{
				case HeadingRenderer:
					ObjectRenderers[i] = new HeadingOverrideRenderer();
					break;
				case PrismCodeBlockRenderer renderer:
					ObjectRenderers[i] =
						new PrismCodeBlockOverrideRenderer(
							(CodeBlockRenderer)renderer.GetType()
								.GetField("codeBlockRenderer", BindingFlags.Instance | BindingFlags.NonPublic)
								!.GetValue(renderer)
						);
					break;
				default:
					continue;
			}
		}
	}

	public void DoReset() => Reset();
}