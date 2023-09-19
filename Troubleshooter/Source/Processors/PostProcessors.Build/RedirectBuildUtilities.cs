using System.IO;
using System.Text.RegularExpressions;

namespace Troubleshooter;

public static class RedirectBuildUtilities
{
	public static void CopyFileIfDifferentAndRedirectInternalLinks(FileInfo from, string to)
	{
		string text = File.ReadAllText(from.FullName);

		MatchCollection matches = CommonRegex.LoadPage.Matches(text);
		// No links to redirect
		if (matches.Count == 0)
		{
			IOUtility.WriteFileTextIfDifferent(text, from, to);
			return;
		}

		string toDirectory = Path.GetDirectoryName(to)!;
		string fromDirectory = Path.GetDirectoryName(from.FullName)!;
		
		// TODO redirect local site links that don't reach destinations
		text = StringUtility.ReplaceMatch(text, matches, (match, builder) =>
		{
			// TODO check if link is in redirected directory
			if (match.Groups[1].Value.StartsWith('/'))
			{
				builder.Append(match.Groups[0].ValueSpan);
				return;
			}

			// Redirect local site links
			string relativePath = Path.GetRelativePath(toDirectory, Path.Combine(fromDirectory, match.Groups[1].Value));
			builder.Append("loadPage('");
			builder.Append(relativePath);
			builder.Append("')");
		});


		// TODO check if different via text. WriteFileTextIfDifferent
		IOUtility.WriteFileTextIfDifferent(text, from, to);
	}
}