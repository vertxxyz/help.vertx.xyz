using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Replaces com.unity links with a deep link that can add the package via UMP
/// </summary>
[UsedImplicitly]
public sealed partial class UpmPackageLinkReplacement : IHtmlPostProcessor
{
	[GeneratedRegex(@"<code>(com\.[\w.\-@]+?)</code>", RegexOptions.Multiline)]
	private static partial Regex GetUpmLinkRegex();

	private static readonly Regex s_UpmLinkRegex = GetUpmLinkRegex();

	public string Process(string html, string fullPath) => s_UpmLinkRegex.Replace(html, "<code><a class=\"link--upm\" href=\"com.unity3d.kharma:upmpackage/$1\" title=\"Install $1 via UPM 2021.2+\">$1</a></code>");
}