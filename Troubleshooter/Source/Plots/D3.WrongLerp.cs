using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace Troubleshooter;

public sealed partial class D3
{
	private enum FrameRates
	{
		Fps100 = 100,
		Fps50 = 50,
		Fps20 = 20,
		Fps10 = 10
	}

	private static string TimeToValueGraph(IJavaScriptExecutor webDriver, Func<(float value, float deltaTime), float> map, (float xMin, float xMax) domain, ILogger logger)
	{
		const float origin = 0;
		const float destination = 1;
		const int maxIterations = 100000;
		// rates in ms. 100, 50, 20, 10
		ReadOnlySpan<int> frameRates = stackalloc int[] { (int)FrameRates.Fps100, (int)FrameRates.Fps50, (int)FrameRates.Fps20, (int)FrameRates.Fps10 };
		ReadOnlySpan<float> deltaTimes = stackalloc float[] { 1 / (float)FrameRates.Fps100, 1 / (float)FrameRates.Fps50, 1 / (float)FrameRates.Fps20, 1 / (float)FrameRates.Fps10 };

		// value = Lerp(value, destination, rate);

		var values = new List<TimeToValue>[frameRates.Length];
		for (int i = 0; i < values.Length; i++)
		{
			List<TimeToValue> v = values[i] = [];
			float value = origin;
			float deltaTime = deltaTimes[i];
			float time = 0;
			int iterations = 0;
			do
			{
				v.Add(new TimeToValue(time, value));
				time = (float)Math.Round(time + deltaTime, 2);
				value = map((value, deltaTime));
			} while (Math.Abs(value - destination) > 0.001f && ++iterations < maxIterations);

			v.Add(new TimeToValue(time, 1));
			if (i > 0)
				v.Add(new TimeToValue(values[0][^1].Time, 1)); // Append a final value to the graphs with shorter times.

			logger.LogDebug("- {DeltaTime}dt - {FrameRates}fps -\n{Elements}", deltaTime, frameRates[i], v.ToElementsString());
		}

		webDriver.ExecuteScript(ToJavascriptArrayDeclaration(nameof(FrameRates.Fps100), values[0], "fps: \"100fps\""));
		webDriver.ExecuteScript(ToJavascriptArrayDeclaration(nameof(FrameRates.Fps50), values[1], "fps: \"50fps\""));
		webDriver.ExecuteScript(ToJavascriptArrayDeclaration(nameof(FrameRates.Fps20), values[2], "fps: \"20fps\""));
		webDriver.ExecuteScript(ToJavascriptArrayDeclaration(nameof(FrameRates.Fps10), values[3], "fps: \"10fps\""));

		// language=javascript
		string js =
			$$"""
			  return Plot.plot({
			  	color: {
			  		legend: true,
			  		domain: ["10fps", "20fps", "50fps", "100fps"],
			  		scheme: "Tableau10"
			  	},
			  	x: {
			  		label: "Seconds",
			  		domain: [{{domain.xMin}}, {{domain.xMax}}],
			  		tickFormat: ""
			  	},
			  	y: {
			  		label: "Value",
			  		grid: true
			  	},
			  	marks: [
			  		Plot.ruleY([0]),
			  		Plot.lineY({{nameof(FrameRates.Fps100)}}, {x: "{{TimeToValue.KeyTime}}", y: "{{TimeToValue.KeyValue}}", stroke: "fps"}),
			  		Plot.lineY({{nameof(FrameRates.Fps50)}}, {x: "{{TimeToValue.KeyTime}}", y: "{{TimeToValue.KeyValue}}", stroke: "fps"}),
			  		Plot.lineY({{nameof(FrameRates.Fps20)}}, {x: "{{TimeToValue.KeyTime}}", y: "{{TimeToValue.KeyValue}}", stroke: "fps"}),
			  		Plot.lineY({{nameof(FrameRates.Fps10)}}, {x: "{{TimeToValue.KeyTime}}", y: "{{TimeToValue.KeyValue}}", stroke: "fps"})
			  	],
			  	style: {
			  		background: "none"
			  	},
			  	document: document
			  }).outerHTML;

			  """;

		return (string)webDriver.ExecuteScript(js);
	}

	private static string WrongLerpGraph(IJavaScriptExecutor webDriver, ILogger logger) => TimeToValueGraph(webDriver, i => Maths.LerpUnclamped(i.value, 1, i.deltaTime * 10), (0, 0.5f), logger);

	private static string ImprovedWrongLerpGraphPow(IJavaScriptExecutor webDriver, ILogger logger) => TimeToValueGraph(webDriver, i => Maths.FractionalDamping(i.value, 1, 0.0001f, i.deltaTime), (0, 0.5f), logger);
	private static string ImprovedWrongLerpGraphExp(IJavaScriptExecutor webDriver, ILogger logger) => TimeToValueGraph(webDriver, i => Maths.ExponentialDecay(i.value, 1, 10, i.deltaTime), (0, 0.5f), logger);
}
