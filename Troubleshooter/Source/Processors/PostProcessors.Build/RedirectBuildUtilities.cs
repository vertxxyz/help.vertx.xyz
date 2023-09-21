using System.IO;
using System.Text.RegularExpressions;

namespace Troubleshooter;

public static class RedirectBuildUtilities
{
	public static bool CopyFileIfDifferentAndRedirectInternalLinks(FileInfo from, string to)
	{
		string text = File.ReadAllText(from.FullName);

		MatchCollection matches = CommonRegex.LoadPage.Matches(text);
		
		if (matches.Count > 0)
		{
			string toDirectory = Path.GetDirectoryName(to)!;
			string fromDirectory = Path.GetDirectoryName(from.FullName)!;

			// Redirect local site links so they redirect to the base resource.
			text = StringUtility.ReplaceMatch(text, matches, (match, builder) =>
			{
				if (match.Groups[1].Value.StartsWith('/'))
				{
					builder.Append(match.Groups[0].ValueSpan);
					return;
				}

				// Redirect local site links
				string relativePath = Path.GetRelativePath(toDirectory, Path.GetFullPath(match.Groups[1].Value, fromDirectory)).ToOutputPath();
				builder.Append("loadPage('");
				builder.Append(relativePath);
				builder.Append("')");
			});
		}


		// TODO check if different via text. WriteFileTextIfDifferent
		return IOUtility.WriteFileTextIfDifferent(text, to);
	}
}