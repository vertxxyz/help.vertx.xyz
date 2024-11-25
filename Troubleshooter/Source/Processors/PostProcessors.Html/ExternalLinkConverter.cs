using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Adds the class "link--external" to external links.
/// </summary>
[UsedImplicitly]
public sealed partial class ExternalLinkConverter : IHtmlPostProcessor
{
	[GeneratedRegex("(?<=<a )href=\"https?://[.\\w/\\-%#?=@_]+\"")]
	private static partial Regex ExternalLinkRegex { get; }

	public int Order => 1;

	public string Process(string html, string fullPath) =>
		StringUtility.ReplaceMatch(html, ExternalLinkRegex, (group, stringBuilder) =>
		{
			stringBuilder.Append("""class="link--external" """);
			stringBuilder.Append(group);
		});
}
