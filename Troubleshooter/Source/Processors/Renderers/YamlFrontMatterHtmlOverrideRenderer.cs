using System;
using Markdig.Extensions.Yaml;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Troubleshooter.Renderers;

public class YamlFrontMatterHtmlOverrideRenderer : HtmlObjectRenderer<YamlFrontMatterBlock>
{
	protected override void Write(HtmlRenderer renderer, YamlFrontMatterBlock obj)
	{
		foreach (StringLine line in obj.Lines.Lines)
		{
			string text = line.Slice.ToString();
			if (string.IsNullOrWhiteSpace(text)) continue;

			if (text.StartsWith("title: ", StringComparison.OrdinalIgnoreCase))
			{
				string title = text["title: ".Length..].Trim('"').Trim();
				renderer.Write($"""<div class="hidden"><h1>{HeadingOverrideRenderer.HeaderTextTag}{title}</span></h1></div>""");
			}
		}
	}
}
