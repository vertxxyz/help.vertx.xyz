using System;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Troubleshooter
{
	public static class StringUtility
	{
		public static string ReplaceMatch(string text, [RegexPattern] string pattern, Func<string, string> matchRemap)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int last = 0;
			MatchCollection matches = Regex.Matches(text, pattern);
			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];
				Group group = match.Groups[1];
				stringBuilder.Append(text[last..match.Index]);
				stringBuilder.Append(matchRemap.Invoke(group.Value));
				last = match.Index + match.Length;
			}
			stringBuilder.Append(text[last..]);
			return stringBuilder.ToString();
		}	
	}
}