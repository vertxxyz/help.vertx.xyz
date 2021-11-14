using Markdig.Prism;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Troubleshooter.Renderers;

public class PrismCodeBlockOverrideRenderer : PrismCodeBlockRenderer
{
	public PrismCodeBlockOverrideRenderer(CodeBlockRenderer codeBlockRenderer) : base(codeBlockRenderer) { }

	protected override void Write(HtmlRenderer renderer, CodeBlock node) => HtmlUtility.AppendWithCodeBlockSetup(renderer, () => base.Write(renderer, node));
}