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
	private static readonly TwemojiLib s_Twemoji = new();

	private static readonly string[] s_DontReplace = { "⤴" };
	private static (string value, string temp)[]? s_DontReplaceLookup;

	public string Process(string html, string fullPath)
	{
		if (s_DontReplaceLookup == null)
		{
			// Generate lookup.
			s_DontReplaceLookup = new (string value, string temp)[s_DontReplace.Length];
			for (var i = 0; i < s_DontReplace.Length; i++)
				s_DontReplaceLookup[i] = (s_DontReplace[i], Guid.NewGuid().ToString());
		}

		// Just parse if there's no values to ignore in the html.
		if (!s_DontReplace.Any(value => html.Contains(value)))
			return s_Twemoji.Parse(html);

		// Replace with temporary values.
		foreach ((string value, string temp) in s_DontReplaceLookup)
			html = html.Replace(value, temp);

		html = s_Twemoji.Parse(html);

		// Replace values back.
		foreach ((string value, string temp) in s_DontReplaceLookup)
			html = html.Replace(temp, value);

		return html;
	}
}