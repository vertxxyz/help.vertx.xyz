using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Troubleshooter
{
	public static class StringUtility
	{
		public static string ReplaceMatch(string text, Regex pattern, Action<string, StringBuilder> matchRemap, int groupIndex = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int last = 0;
			MatchCollection matches = pattern.Matches(text);
			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];
				Group group = match.Groups[groupIndex];
				stringBuilder.Append(text[last..match.Index]);
				matchRemap.Invoke(group.Value, stringBuilder);
				last = match.Index + match.Length;
			}
			stringBuilder.Append(text[last..]);
			return stringBuilder.ToString();
		}	
	}
}