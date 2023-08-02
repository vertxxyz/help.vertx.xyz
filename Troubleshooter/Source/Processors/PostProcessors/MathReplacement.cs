using System.Text.RegularExpressions;
using JavaScriptEngineSwitcher.V8;
using JetBrains.Annotations;

namespace Troubleshooter;

[UsedImplicitly]
public sealed partial class MathReplacement : IHtmlPostProcessor
{
	[GeneratedRegex("""
	                <span class="math">\\\((.+?)\\\)</span>
	                """)]
	private static partial Regex GetMathRegex();

	private static readonly Regex s_MathRegex = GetMathRegex();

	private readonly V8JsEngine _engine;

	public MathReplacement()
	{
		_engine = new V8JsEngine();
		_engine.ExecuteResource("Katex", typeof(Program).Assembly);
	}

	public string Process(string html, string fullPath)
	{
		if (!s_MathRegex.IsMatch(html))
			return html;

		MatchCollection matches = s_MathRegex.Matches(html);
		foreach (Match match in matches)
		{
			_engine.SetVariableValue("text", match.Groups[1].Value);
			_engine.Execute("mathOut = katex.renderToString(text, { throwOnError: false })");

			string mathOut = _engine.Evaluate<string>("mathOut");
			html = html.Replace(match.Value, $"<span class=\"math\">{mathOut}</span>");
		}

		return html;
	}
}