using System.Globalization;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Troubleshooter.Renderers
{
	public class HeadingOverrideRenderer : HeadingRenderer
	{
		private static readonly string[] HeadingTexts = {
			"h1",
			"h2",
			"h3",
			"h4",
			"h5",
			"h6",
		};

		protected override void Write(HtmlRenderer renderer, HeadingBlock obj)
		{
			int index = obj.Level - 1;
			string headingText = (uint)index < (uint)HeadingTexts.Length
				? HeadingTexts[index]
				: $"h{obj.Level.ToString(CultureInfo.InvariantCulture)}";

			if (renderer.EnableHtmlForBlock)
			{
				renderer.Write("<").Write(headingText).WriteAttributes(obj).Write('>');
				renderer.Write("<span class =\"header-text\">");
			}

			renderer.WriteLeafInline(obj);

			if (renderer.EnableHtmlForBlock)
			{
				renderer.Write("</span>");
				// Append Hash permalink to ID if one exists.
				string id = obj.TryGetAttributes()?.Id;
				if (!string.IsNullOrEmpty(id))
				{
					renderer.Write("<a class=\"header-permalink\" onclick=\"loadHash('#").Write(id).Write("')\">#</a>");
				}

				renderer.Write("</").Write(headingText).WriteLine(">");
			}

			renderer.EnsureLine();
		}
	}
}