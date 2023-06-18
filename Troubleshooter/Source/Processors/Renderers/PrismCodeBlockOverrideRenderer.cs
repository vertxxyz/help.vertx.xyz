using System;
using System.Text;
using JavaScriptEngineSwitcher.V8;
using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Troubleshooter.Renderers;

public static class PrismExtensions
{
	public static MarkdownPipelineBuilder UsePrism(this MarkdownPipelineBuilder pipeline)
	{
		pipeline.Extensions.Add(new PrismExtension());
		return pipeline;
	}
}

public class PrismExtension : IMarkdownExtension
{
	public void Setup(MarkdownPipelineBuilder pipeline) { }

	public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
	{
		if (renderer == null)
			throw new ArgumentNullException(nameof(renderer));
		if (renderer is not TextRendererBase<HtmlRenderer> textRendererBase)
			return;
		CodeBlockRenderer? codeBlockRenderer = textRendererBase.ObjectRenderers.FindExact<CodeBlockRenderer>();
		if (codeBlockRenderer != null)
			textRendererBase.ObjectRenderers.Remove(codeBlockRenderer);
		textRendererBase.ObjectRenderers.AddIfNotAlready(new PrismCodeBlockRenderer(codeBlockRenderer));
	}
}

public class PrismCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
{
	private readonly CodeBlockRenderer codeBlockRenderer;
	private readonly V8JsEngine _engine;

	public PrismCodeBlockRenderer(CodeBlockRenderer? codeBlockRenderer)
	{
		this.codeBlockRenderer = codeBlockRenderer ?? new CodeBlockRenderer();
		_engine = new V8JsEngine();
		_engine.ExecuteResource("Prism", typeof(Program).Assembly);
	}

	protected override void Write(HtmlRenderer renderer, CodeBlock node)
	{
		var fencedCodeBlock = node as FencedCodeBlock;
		// Bypass prism if this block should just render as a div.
		if (fencedCodeBlock?.Info != null && codeBlockRenderer.BlocksAsDiv.Contains(fencedCodeBlock.Info))
		{
			codeBlockRenderer.Write(renderer, node);
			return;
		}

		HtmlUtility.AppendWithCodeBlockSetup(renderer, () => Render(renderer, node));
	}

	private void Render(HtmlRenderer renderer, CodeBlock node)
	{
		if (node is not FencedCodeBlock fencedCodeBlock || node.Parser is not FencedCodeBlockParser parser)
		{
			codeBlockRenderer.Write(renderer, node);
		}
		else
		{
			string str = fencedCodeBlock.Info!.Replace(parser.InfoPrefix!, string.Empty);
			switch (str)
			{
				case "cs":
				case "csharp":
				{
					_engine.SetVariableValue("sourceCode", ExtractSourceCode(node));
					_engine.SetVariableValue("language", str);
					_engine.Execute("highlighted = Prism.highlight(sourceCode, Prism.languages.csharp, language)");
					
					string highlightedSourceCode = _engine.Evaluate<string>("highlighted");
					
					renderer.Write("<pre>").Write("<code").Write(">").Write(highlightedSourceCode).Write("</code>").Write("</pre>");
					break;
				}
				default:
					codeBlockRenderer.Write(renderer, node);
					break;
			}
		}
	}

	private static string ExtractSourceCode(LeafBlock node)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringLine[] lines = node.Lines.Lines;
		int length = lines.Length;
		for (int index = 0; index < length; ++index)
		{
			StringSlice slice = lines[index].Slice;
			if (slice.Text != null)
			{
				string str = slice.Text.Substring(slice.Start, slice.Length);
				if (index > 0)
					stringBuilder.AppendLine();
				stringBuilder.Append(str);
			}
		}

		return stringBuilder.ToString();
	}
}