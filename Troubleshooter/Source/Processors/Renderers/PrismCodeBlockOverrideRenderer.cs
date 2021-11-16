using Markdig.Prism;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Troubleshooter.Renderers;

public class PrismCodeBlockOverrideRenderer : PrismCodeBlockRenderer
{
	private readonly CodeBlockRenderer codeBlockRenderer;

	public PrismCodeBlockOverrideRenderer(CodeBlockRenderer codeBlockRenderer) : base(codeBlockRenderer)
		=> this.codeBlockRenderer = codeBlockRenderer;

	protected override void Write(HtmlRenderer renderer, CodeBlock node)
	{
		var fencedCodeBlock = node as FencedCodeBlock;
		// Bypass prism if this block should just render as a div.
		if (fencedCodeBlock?.Info != null && codeBlockRenderer.BlocksAsDiv.Contains(fencedCodeBlock.Info))
		{
			codeBlockRenderer.Write(renderer, node);
			return;
		}

		HtmlUtility.AppendWithCodeBlockSetup(renderer, () => base.Write(renderer, node));
	}
}