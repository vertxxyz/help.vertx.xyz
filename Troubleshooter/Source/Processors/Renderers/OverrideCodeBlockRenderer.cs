using System;
using System.Text;
using JavaScriptEngineSwitcher.Core;
using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Troubleshooter.Renderers;

public static class CodeHighlightingExtensions
{
	public static MarkdownPipelineBuilder UseCodeHighlighting(this MarkdownPipelineBuilder pipeline, IServiceProvider provider)
	{
		pipeline.Extensions.Add(new CodeHighlightingExtension(provider));
		return pipeline;
	}
}

public class CodeHighlightingExtension : IMarkdownExtension
{
	private readonly IServiceProvider _provider;

	public CodeHighlightingExtension(IServiceProvider provider)
	{
		_provider = provider;
	}

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
		textRendererBase.ObjectRenderers.AddIfNotAlready(new OverrideCodeBlockRenderer(
			codeBlockRenderer, 
			_provider.GetRequiredService<IJsEngine>(),
			ActivatorUtilities.CreateInstance<D3>(_provider),
			ActivatorUtilities.CreateInstance<Mermaid>(_provider),
			ActivatorUtilities.CreateInstance<Nomnoml>(_provider),
			_provider.GetRequiredService<ILogger<OverrideCodeBlockRenderer>>()
		));
	}
}

public class OverrideCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
{
	private readonly CodeBlockRenderer _codeBlockRenderer;
	private readonly IJsEngine _engine;
	private readonly D3 _d3;
	private readonly Mermaid _mermaid;
	private readonly Nomnoml _nomnoml;
	private readonly ILogger _logger;

	public OverrideCodeBlockRenderer(CodeBlockRenderer? codeBlockRenderer, IJsEngine jsEngine, D3 d3, Mermaid mermaid, Nomnoml nomnoml, ILogger<OverrideCodeBlockRenderer> logger)
	{
		_d3 = d3;
		_mermaid = mermaid;
		_nomnoml = nomnoml;
		_logger = logger;
		_codeBlockRenderer = codeBlockRenderer ?? new CodeBlockRenderer();
		_codeBlockRenderer.BlocksAsDiv.Add("d3");
		_codeBlockRenderer.BlocksAsDiv.Add("mermaid");
		_codeBlockRenderer.BlocksAsDiv.Add("nomnoml");

		// This is a local resource as it's configured using a specific setup before being downloaded.
		_engine = jsEngine;
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
			string language = fencedCodeBlock.Info!.Replace(parser.InfoPrefix!, string.Empty);
			switch (language)
			{
				case "cs":
				case "csharp":
					Highlight(language, "csharp");
					break;
				case "diff":
				case "css":
				case "json":
				case "cmake":
					Highlight(language, language);
					break;
				case "d3":
					_d3.Plot(ExtractSourceCode(node), renderer);
					break;
				case "mermaid":
					_mermaid.Plot(ExtractSourceCode(node), renderer);
					break;
				case "nomnoml":
					_nomnoml.Plot(ExtractSourceCode(node), renderer);
					break;
				case "":
					_codeBlockRenderer.Write(renderer, node);
					break;
				default:
					_logger.LogWarning("{Language} was not highlighted or plotted, and isn't added as a default, was this a mistake?", language);
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