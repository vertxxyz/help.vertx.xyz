using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Troubleshooter
{
	public static class StringUtility
	{
		public static string ReplaceMatch(string text, Regex pattern, Action<string, StringBuilder> matchRemap, int groupIndex)
		{
			int last = 0;
			MatchCollection matches = pattern.Matches(text);
			if (matches.Count == 0) return text;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];
				stringBuilder.Append(text[last..match.Index]);
				matchRemap.Invoke(match.Groups[groupIndex].Value, stringBuilder);
				last = match.Index + match.Length;
			}

			stringBuilder.Append(text[last..]);
			return stringBuilder.ToString();
		}
		
		public static string ReplaceMatch(string text, Regex pattern, Action<Match, StringBuilder> matchRemap)
		{
			int last = 0;
			MatchCollection matches = pattern.Matches(text);
			if (matches.Count == 0) return text;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < matches.Count; i++)
			{
				Match match = matches[i];
				stringBuilder.Append(text[last..match.Index]);
				matchRemap.Invoke(match, stringBuilder);
				last = match.Index + match.Length;
			}

			stringBuilder.Append(text[last..]);
			return stringBuilder.ToString();
		}


		public static string ToLowerSnakeCase(string text)
		{
			text = text.ToLower();
			text = text.Replace("%20", "-");
			text = text.Replace(" ", "-");
			return text;
		}
	}
}