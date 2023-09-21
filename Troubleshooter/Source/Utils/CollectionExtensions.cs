using System;
using System.Collections.Generic;
using System.Text;

namespace Troubleshooter;

public static class CollectionExtensions
{
	public static string ToElementsString<T>(this IEnumerable<T> enumerable, Func<T, string>? remap = null)
	{
		StringBuilder result = new();
		foreach (T t in enumerable)
		{
			result.AppendLine(
				remap == null ? t == null ? "null" : t.ToString() : remap(t)
			);
		}

		if (result.Length > 0)
		{
			int removeLength = Environment.NewLine.Length;
			result.Remove(result.Length - removeLength, removeLength);
		}

		return result.ToString();
	}

	public static IEnumerator<T> GetEnumerator<T>(this IEnumerator<T> enumerator) => enumerator;
}
