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

public static class CodeHighlightingExtensions
{
	public static MarkdownPipelineBuilder UseCodeHighlighting(this MarkdownPipelineBuilder pipeline)
	{
		pipeline.Extensions.Add(new CodeHighlightingExtension());
		return pipeline;
	}
}

public class CodeHighlightingExtension : IMarkdownExtension
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
		textRendererBase.ObjectRenderers.AddIfNotAlready(new OverrideCodeBlockRenderer(codeBlockRenderer));
	}
}

public class OverrideCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
{
	private readonly CodeBlockRenderer _codeBlockRenderer;
	private readonly V8JsEngine _engine;

	public OverrideCodeBlockRenderer(CodeBlockRenderer? codeBlockRenderer)
	{
		_codeBlockRenderer = codeBlockRenderer ?? new CodeBlockRenderer();
		_codeBlockRenderer.BlocksAsDiv.Add("d3");
		_engine = new V8JsEngine();
		_engine.ExecuteResource("Prism", typeof(Program).Assembly);
	}

	protected override void Write(HtmlRenderer renderer, CodeBlock node)
	{
		var fencedCodeBlock = node as FencedCodeBlock;
		// Bypass code highlighting if this block should just render as a div.
		if (fencedCodeBlock?.Info != null && _codeBlockRenderer.BlocksAsDiv.Contains(fencedCodeBlock.Info))
		{
			Render(renderer, node);
			return;
		}

		HtmlUtility.AppendWithCodeBlockSetup(renderer, () => Render(renderer, node));
	}

	private void Render(HtmlRenderer renderer, CodeBlock node)
	{
		if (node is not FencedCodeBlock fencedCodeBlock || node.Parser is not FencedCodeBlockParser parser)
		{
			_codeBlockRenderer.Write(renderer, node);
		}
		else
		{
			string str = fencedCodeBlock.Info!.Replace(parser.InfoPrefix!, string.Empty);
			switch (str)
			{
				case "cs":
				case "csharp":
					Highlight(str, "csharp");
					break;
				case "diff":
					Highlight(str, "diff");
					break;
				case "css":
					Highlight(str, "css");
					break;
				case "d3":
					D3.Plot(ExtractSourceCode(node), renderer);
					break;
				default:
					_codeBlockRenderer.Write(renderer, node);
					break;
			}
		}

		return;

		void Highlight(string language, string languageKey)
		{
			_engine.SetVariableValue("sourceCode", ExtractSourceCode(node));
			_engine.SetVariableValue("language", language);
			_engine.Execute($"highlighted = Prism.highlight(sourceCode, Prism.languages.{languageKey}, language)");

			string highlightedSourceCode = _engine.Evaluate<string>("highlighted");

			renderer.Write($"<pre class=\"{languageKey}\">").Write("<code").Write(">").Write(highlightedSourceCode).Write("</code>").Write("</pre>");
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
			if (slice.Text == null)
				continue;
			string str = slice.Text.Substring(slice.Start, slice.Length);
			if (index > 0)
				stringBuilder.AppendLine();
			stringBuilder.Append(str);
		}

		return stringBuilder.ToString();
	}
}