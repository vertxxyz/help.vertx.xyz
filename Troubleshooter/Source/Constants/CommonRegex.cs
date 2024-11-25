using System.Text.RegularExpressions;

namespace Troubleshooter;

public static partial class CommonRegex
{
	/// <summary>
	/// Regex for links: [$1]($2)
	/// </summary>
	[GeneratedRegex(@"(?<!!)\[(.+?)\]\(([\w /%.#-]+)\)")]
	public static partial Regex InternalLinks { get; }

	/// <summary>
	/// Regex for external links: ...]($1)
	/// </summary>
	[GeneratedRegex(@"]\((https?://[\w/%#?.@_+~=&()]+)\)")]
	public static partial Regex ExternalLink { get; }

	/// <summary>
	/// Regex for embeds: &lt;&lt;$1&gt;&gt;
	/// </summary>
	[GeneratedRegex(@"<<([A-Za-z0-9\-/ ]+?\.[a-zA-z]+?)>>")]
	public static partial Regex Embeds { get; }

	/// <summary>
	/// Regex for local image links: ![]($1)
	/// </summary>
	[GeneratedRegex("""!\[[^\]]*\]\((?!http)(.*?)\s*(".*[^"]")?\s*\)""")]
	public static partial Regex LocalImages { get; }
}
