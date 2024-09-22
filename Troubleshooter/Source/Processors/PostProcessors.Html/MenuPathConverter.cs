using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter;

/// <summary>
/// Converts **Path | To | Thing** to use a path separator character inside of a div.
/// </summary>
[UsedImplicitly]
public sealed partial class MenuPathConverter : IHtmlPostProcessor
{
	[GeneratedRegex(@"<strong>([^|<]+ \| [^<]+?)</strong>")]
	private static partial Regex GetMenuPathRegex();

	private static readonly Regex s_menuPathRegex = GetMenuPathRegex();

	public string Process(string html, string fullPath) =>
		StringUtility.ReplaceMatch(html, s_menuPathRegex, (group, stringBuilder) =>
		{
			stringBuilder.Append("<span class=\"menu-path\">");
			{
				string[] pathElements = group.Split(" | ");
				for (int i = 0; i < pathElements.Length; i++)
				{
					string element = pathElements[i];
					stringBuilder.Append("<span class=\"menu-path__item\">");
					stringBuilder.Append(element);
					stringBuilder.Append("</span>");

					if (i != pathElements.Length - 1)
					{
						stringBuilder.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" class=\"menu-path__separator\"><use href=\"#menu-path-separator-icon\"></use></svg>");
					}
				}
			}
			stringBuilder.Append("</span>");
		}, 1);
}
