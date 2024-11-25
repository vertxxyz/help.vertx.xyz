using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Troubleshooter;

public static partial class StringUtility
{
	public static string ReplaceMatch(string text, Regex pattern, Action<string, StringBuilder> matchRemap, int groupIndex)
	{
		int last = 0;
		MatchCollection matches = pattern.Matches(text);
		if (matches.Count == 0) return text;
		StringBuilder stringBuilder = new();
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
		MatchCollection matches = pattern.Matches(text);
		if (matches.Count == 0) return text;
		int last = 0;
		StringBuilder stringBuilder = new();
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

	public static string ReplaceMatch(string text, MatchCollection matches, Action<Match, StringBuilder> matchRemap)
	{
		int last = 0;
		StringBuilder stringBuilder = new(text.Length);
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

	[GeneratedRegex("^(.*)$", RegexOptions.Multiline)]
	private static partial Regex FirstLine { get; }

	public static ReadOnlySpan<char> LineAt(string input, int index) => FirstLine.Match(input[index..]).Groups[1].ValueSpan;

	public static int LastIndexOf(this StringBuilder stringBuilder, char character)
		=> stringBuilder.LastIndexOf(character, stringBuilder.Length - 1);

	public static int LastIndexOf(this StringBuilder stringBuilder, char character, int start)
	{
		int index = start;
		while (index >= 0 && stringBuilder[index] != character)
			index--;
		return index < 0 ? -1 : index;
	}

	public static int NextIndexOf(this StringBuilder stringBuilder, char character, int start)
	{
		int index = start;
		while (index >= 0 && index < stringBuilder.Length && stringBuilder[index] != character)
			index++;
		return index >= stringBuilder.Length ? -1 : index;
	}

	public static int NextIndexOf(this StringBuilder stringBuilder, string value, int start)
	{
		int index = start;
		while (index >= 0 && index + value.Length <= stringBuilder.Length && !ContainsAt(stringBuilder, value, index))
			index++;
		return index + value.Length <= stringBuilder.Length ? index : -1;
	}

	private static bool ContainsAt(this StringBuilder stringBuilder, string value, int index)
	{
		for (int i = 0, j = index; i < value.Length; i++, j++)
		{
			if (j > stringBuilder.Length)
				return false;
			if (stringBuilder[j] != value[i])
				return false;
		}

		return true;
	}
}
