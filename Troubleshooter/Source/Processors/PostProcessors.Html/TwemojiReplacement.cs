using System;
using System.Linq;
using JetBrains.Annotations;
using TwemojiSharp;

namespace Troubleshooter;

/// <summary>
/// Replaces all emoji with Twemoji.
/// </summary>
[UsedImplicitly]
public sealed class TwemojiReplacement : IHtmlPostProcessor
{
	private static readonly TwemojiLib s_twemoji = new();

	private static readonly string[] s_dontReplace = ["⤴"];
	private static (string value, string temp)[]? s_dontReplaceLookup;

	public string Process(string html, string fullPath)
	{
		if (s_dontReplaceLookup == null)
		{
			// Generate lookup.
			s_dontReplaceLookup = new (string value, string temp)[s_dontReplace.Length];
			for (var i = 0; i < s_dontReplace.Length; i++)
				s_dontReplaceLookup[i] = (s_dontReplace[i], Guid.NewGuid().ToString());
		}

		// Just parse if there's no values to ignore in the html.
		if (!s_dontReplace.Any(value => html.Contains(value)))
			return s_twemoji.Parse(html);

		// Replace with temporary values.
		foreach ((string value, string temp) in s_dontReplaceLookup)
			html = html.Replace(value, temp);

		html = s_twemoji.Parse(html);

		// Replace values back.
		foreach ((string value, string temp) in s_dontReplaceLookup)
			html = html.Replace(temp, value);

		return html;
	}
}
