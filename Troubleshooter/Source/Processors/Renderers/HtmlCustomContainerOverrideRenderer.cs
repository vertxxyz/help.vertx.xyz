using System.Collections.Generic;
using Markdig.Extensions.CustomContainers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Troubleshooter.Renderers;

public class HtmlCustomContainerOverrideRenderer : HtmlObjectRenderer<CustomContainer>
{
	private readonly List<(string @class, string imageUrl, string alt)> _infoBoxes =
	[
		("info", "https://unity.huh.how/Images/information.svg", "information"),
		("warning", "https://unity.huh.how/Images/warning.svg", "warning"),
		("error", "https://unity.huh.how/Images/error.svg", "error")
	];

	protected override void Write(HtmlRenderer renderer, CustomContainer obj)
	{
		renderer.EnsureLine();

		HtmlAttributes? attributes = obj.TryGetAttributes();
		if (renderer.EnableHtmlForBlock)
		{
			if (attributes is { Classes: not null })
			{
				// Convert divs with the correct classes into info boxes.
				foreach ((string @class, string imageUrl, string alt) in _infoBoxes)
				{
					if (!attributes.Classes.Contains(@class))
						continue;

					attributes.AddClass("info-box");
					renderer.Write("<div").WriteAttributes(attributes).Write('>');

					renderer.Write("<img src=\"");
					renderer.Write(imageUrl);
					renderer.Write("\" class=\"info\" alt=\"");
					renderer.Write(alt);
					renderer.Write("\" />");

					// We always want to write a paragraph inside of an info block.
					renderer.ImplicitParagraph = false;
					renderer.WriteChildren(obj);

					renderer.WriteLine("</div>");
					return;
				}
			}

			renderer.Write("<div").WriteAttributes(attributes).Write('>');
		}

		// We don't escape a CustomContainer
		renderer.WriteChildren(obj);
		if (renderer.EnableHtmlForBlock)
		{
			renderer.WriteLine("</div>");
		}
	}
}