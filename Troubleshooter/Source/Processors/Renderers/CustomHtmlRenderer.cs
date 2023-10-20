using System.IO;
using Markdig.Extensions.CustomContainers;
using Markdig.Extensions.Yaml;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Troubleshooter.Renderers;

public class CustomHtmlRenderer : HtmlRenderer
{
	public CustomHtmlRenderer(TextWriter writer) : base(writer)
	{
		ObjectRenderers.Add(new YamlFrontMatterHtmlOverrideRenderer());
	}

	public void Setup()
	{
		for (int i = ObjectRenderers.Count - 1; i >= 0; i--)
		{
			switch (ObjectRenderers[i])
			{
				case HeadingRenderer:
					ObjectRenderers[i] = new HeadingOverrideRenderer();
					break;
				case HtmlCustomContainerRenderer:
					ObjectRenderers[i] = new HtmlCustomContainerOverrideRenderer();
					break;
				case YamlFrontMatterHtmlRenderer:
					ObjectRenderers.RemoveAt(i);
					break;
				default:
					continue;
			}
		}
	}

	public void DoReset() => Reset();
}
