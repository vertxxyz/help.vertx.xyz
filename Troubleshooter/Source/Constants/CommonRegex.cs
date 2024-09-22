using System.Text.RegularExpressions;

namespace Troubleshooter;

public static partial class CommonRegex
{
	/// <summary>
	/// Regex for links: [$1]($2)
	/// </summary>
	public static readonly Regex InternalLinks = GetInternalLinkRegex();

	/// <summary>
	/// Regex for external links: ...]($1)
	/// </summary>
	public static readonly Regex ExternalLink = GetExternalLinkRegex();

	/// <summary>
	/// Regex for embeds: &lt;&lt;$1&gt;&gt;
	/// </summary>
	public static readonly Regex Embeds = GetEmbedsRegex();

	/// <summary>
	/// Regex for local image links: ![]($1)
	/// </summary>
	public static readonly Regex LocalImages = GetLocalImagesRegex();

	[GeneratedRegex(@"(?<!!)\[(.+?)\]\(([\w /%.#-]+)\)")]
	private static partial Regex GetInternalLinkRegex();

	[GeneratedRegex(@"]\((https?://[\w/%#?.@_+~=&()]+)\)")]
	private static partial Regex GetExternalLinkRegex();

	[GeneratedRegex(@"<<([A-Za-z0-9\-/ ]+?\.[a-zA-z]+?)>>")]
	private static partial Regex GetEmbedsRegex();

	[GeneratedRegex("""!\[[^\]]*\]\((?!http)(.*?)\s*(".*[^"]")?\s*\)""")]
	private static partial Regex GetLocalImagesRegex();
}
