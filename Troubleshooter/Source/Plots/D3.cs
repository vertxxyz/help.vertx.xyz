using System;
using System.Collections.Generic;
using System.Text;
using AdvancedStringBuilder;
using JetBrains.Annotations;
using Markdig.Renderers;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace Troubleshooter;

[UsedImplicitly]
public sealed partial class D3
{
	private readonly WebRenderer _webRenderer;
	private readonly ILogger<D3> _logger;

	public D3(WebRenderer webRenderer, OnlineResources resources, ILogger<D3> logger)
	{
		_webRenderer = webRenderer;
		_logger = logger;
		WebDriver webDriver = _webRenderer.Driver;
		webDriver.ExecuteScript(resources.D3);
		webDriver.ExecuteScript(resources.Plot);
	}

	public void Plot(string key, HtmlRenderer renderer)
	{
		WebDriver webDriver = _webRenderer.Driver;
		string svg = key switch
		{
			"graph-wrong-lerp" => WrongLerpGraph(webDriver, _logger),
			"graph-improved-wrong-lerp" => ImprovedWrongLerpGraphExp(webDriver, _logger),
			_ => throw new ArgumentOutOfRangeException(key, $"{key} is not yet supported by {nameof(D3)}.{nameof(Plot)}.")
		};

		renderer.Write("<div class=\"d3\">").Write(svg).Write("</div>");
	}

	private readonly struct TimeToValue
	{
		public const string KeyTime = "Time";
		public const string KeyValue = "Value";
		public readonly float Time;
		public readonly float Value;

		public TimeToValue(float time, float value)
		{
			Time = time;
			Value = value;
		}

		public override string ToString() => $$"""{{{KeyTime}}: {{Time:0.00}}, {{KeyValue}}: {{Value:0.0000}}}""";
		public string ToString(string appended) => string.IsNullOrEmpty(appended) ? ToString() : $$"""{{{KeyTime}}: {{Time:0.00}}, {{KeyValue}}: {{Value:0.0000}}, {{appended}}}""";
	}

	private static string ToJavascriptArrayDeclaration(string name, List<TimeToValue> values, string appended)
	{
		if (values.Count == 0)
			return $"{name} = [];";

		StringBuilder builder = new(name.Length + 6 + (values[0].ToString().Length + 5) * values.Count + 6);
		builder.Append(name);
		builder.AppendLine(" = [");
		foreach (TimeToValue value in values)
		{
			builder.Append(value.ToString(appended));
			builder.AppendLine(",");
		}

		builder.TrimEnd();
		builder.Remove(builder.Length - 1, 1); // Remove last comma
		builder.AppendLine();
		builder.AppendLine("];");
		return builder.ToString();
	}
}
