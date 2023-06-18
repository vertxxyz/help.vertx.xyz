using System.IO;
using Markdig.Extensions.CustomContainers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Troubleshooter.Renderers;

public class CustomHtmlRenderer : HtmlRenderer
{
	public CustomHtmlRenderer(TextWriter writer) : base(writer) { }

	public void Setup()
	{
		for (int i = 0; i < ObjectRenderers.Count; i++)
		{
			switch (ObjectRenderers[i])
			{
				case HeadingRenderer:
					ObjectRenderers[i] = new HeadingOverrideRenderer();
					break;
				case HtmlCustomContainerRenderer:
					ObjectRenderers[i] = new HtmlCustomContainerOverrideRenderer();
					break;
				default:
					continue;
			}
		}
	}

	public void DoReset() => Reset();
}