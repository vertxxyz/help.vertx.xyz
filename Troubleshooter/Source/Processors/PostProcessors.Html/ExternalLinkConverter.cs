using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Adds the class "link--external" to external links.
/// </summary>
[UsedImplicitly]
public sealed partial class ExternalLinkConverter : IHtmlPostProcessor
{
	[GeneratedRegex("(?<=<a )href=\"https?://[.\\w/\\-%#?=@_]+\"", RegexOptions.Compiled)]
	private static partial Regex GetExternalLinkRegex();

	private static readonly Regex s_externalLinkRegex = GetExternalLinkRegex();

	public int Order => 1;

	public string Process(string html, string fullPath) =>
		StringUtility.ReplaceMatch(html, s_externalLinkRegex, (group, stringBuilder) =>
		{
			stringBuilder.Append("""class="link--external" """);
			stringBuilder.Append(group);
		});
}
