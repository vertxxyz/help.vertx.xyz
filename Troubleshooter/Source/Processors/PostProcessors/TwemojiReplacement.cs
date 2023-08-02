using JetBrains.Annotations;
using TwemojiSharp;

namespace Troubleshooter;

/// <summary>
/// Replaces all emoji with Twemoji
/// </summary>
[UsedImplicitly]
public sealed class TwemojiReplacement : IHtmlPostProcessor
{
	private readonly TwemojiLib twemoji = new();

	public string Process(string html, string fullPath) => twemoji.Parse(html);
}