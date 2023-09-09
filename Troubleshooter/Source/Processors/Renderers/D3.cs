using System;
using JavaScriptEngineSwitcher.V8;
using Markdig.Renderers;

namespace Troubleshooter.Renderers;

internal sealed class D3
{
	private static readonly V8JsEngine s_Engine;

	static D3()
	{
		s_Engine = new V8JsEngine();
		s_Engine.ExecuteResource("D3", typeof(Program).Assembly);
	}
	
	public static void Plot(string key, HtmlRenderer renderer)
	{
		Console.WriteLine(key);
		/*_engine.SetVariableValue("sourceCode", ExtractSourceCode(node));
		_engine.SetVariableValue("language", language);
		_engine.Execute($"highlighted = Prism.highlight(sourceCode, Prism.languages.{languageKey}, language)");

		string highlightedSourceCode = _engine.Evaluate<string>("highlighted");

		renderer.Write($"<pre class=\"{languageKey}\">").Write("<code").Write(">").Write(highlightedSourceCode).Write("</code>").Write("</pre>");*/
	}
}