using System.Text.RegularExpressions;
using JavaScriptEngineSwitcher.Core;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Executes Katex on spans classed as math.
/// </summary>
[UsedImplicitly]
public sealed partial class MathReplacement : IHtmlPostProcessor
{
	[GeneratedRegex("""
	                <span class="math">\\\((.+?)\\\)</span>
	                """)]
	private static partial Regex MathRegex { get; }

	private readonly IJsEngine _engine;

	public MathReplacement(IJsEngine engine, OnlineResources onlineResources)
	{
		_engine = engine;
		_engine.Execute(onlineResources.KaTeX);
	}

	public string Process(string html, string fullPath)
	{
		if (!MathRegex.IsMatch(html))
			return html;

		MatchCollection matches = MathRegex.Matches(html);
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
