using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUglify;
using Troubleshooter.Renderers;

namespace Troubleshooter.Search;

public static class SearchGatherer
{
	public class Result
	{
		public IList<string> FilePaths { get; }
		public IList<string> FileHeadings { get; }
		public ImmutableSortedDictionary<string, Dictionary<int, int>> SortedWordsToFileIndexAndCount { get; }

		public Result(IList<string> filePaths, IList<string> fileHeadings, ImmutableSortedDictionary<string, Dictionary<int, int>> sortedWordsToFileIndexAndCount)
		{
			FilePaths = filePaths;
			SortedWordsToFileIndexAndCount = sortedWordsToFileIndexAndCount;
			FileHeadings = fileHeadings;
		}

		public void Deconstruct(
			out IList<string> filePaths,
			out IList<string> fileHeadings,
			out ImmutableSortedDictionary<string, Dictionary<int, int>> sortedWordsToFileIndexAndCount
		)
		{
			filePaths = FilePaths;
			fileHeadings = FileHeadings;
			sortedWordsToFileIndexAndCount = SortedWordsToFileIndexAndCount;
		}
	}

	private static readonly Regex PreRegex = new("<pre>.+?</pre>", RegexOptions.Compiled | RegexOptions.Singleline);

	public static async Task<Result> GenerateSearchResult(string rootDirectory)
	{
		IEnumerable<string> files = Directory.EnumerateFiles(rootDirectory, "*.html", SearchOption.AllDirectories);

		Dictionary<string, int> fileNameToIndex = new();
		List<string> filePaths = new();
		foreach (string s in files)
		{
			if (EndsWithAny(s, SearchCommon.ExcludedFileEndings)) continue;
			string localPath = s[(rootDirectory.Length + 1)..];
			if (!localPath.Contains('\\')) continue; // Do not process root-level files. These files should not be searchable.
			fileNameToIndex[s] = filePaths.Count;
			filePaths.Add($"/{localPath.Replace('\\', '/')}");
		}

		ConcurrentDictionary<int, string> headerText = new ConcurrentDictionary<int, string>();
		ConcurrentDictionary<string, Dictionary<int, int>> wordsToFileIndexAndCount = new();
		await Parallel.ForEachAsync(fileNameToIndex, async (pair, _) =>
		{
			(string file, int index) = pair;
			if (EndsWithAny(file, SearchCommon.ExcludedFileEndings)) return;
			foreach (string word in await ConstructSearchTermsAsync(file, index, headerText))
			{
				wordsToFileIndexAndCount.AddOrUpdate(
					word,
					_ => new Dictionary<int, int>
					{
						{ index, 1 }
					},
					(_, fileIndexToCount) =>
					{
						lock (fileIndexToCount)
						{
							if (!fileIndexToCount.TryGetValue(index, out int value))
								value = 0;
							fileIndexToCount[index] = value + 1;
						}
							
						return fileIndexToCount;
					}
				);
			}
		});

		// Remove common search strings.
		foreach (string commonValue in SearchCommon.CommonValues)
			wordsToFileIndexAndCount.TryRemove(commonValue, out _);

		string[] fileHeaders = new string[headerText.Count];
		foreach ((int key, string value) in headerText)
			fileHeaders[key] = value;


		ImmutableSortedDictionary<string, Dictionary<int, int>> sortedWordsToFileIndexAndCount = wordsToFileIndexAndCount.ToImmutableSortedDictionary();
		return new Result(filePaths, fileHeaders, sortedWordsToFileIndexAndCount);
	}

	private static async Task<IEnumerable<string>> ConstructSearchTermsAsync(string path, int index, ConcurrentDictionary<int, string> indexToHeaderText)
	{
		string html = await File.ReadAllTextAsync(path);
		// Remove code
		html = PreRegex.Replace(html, string.Empty);

		string? headerText = GetHeaderText(html);
		if (headerText != null)
		{
			indexToHeaderText.TryAdd(index, headerText);
			//Console.WriteLine(headerText);
		}
		else
		{
			indexToHeaderText.TryAdd(index, Path.GetFileNameWithoutExtension(path));
			Console.WriteLine($"\"{path}\" header text failed to be added to search index.");
		}

		// Without this wrapping div Uglify fails to parse many forms of HTML.
		UglifyResult result = Uglify.HtmlToText($"<div>{html}</div>", sourceFileName: path);
		if (result.HasErrors)
		{
			Console.WriteLine($"Error while parsing \"{path}\" to text.");
			foreach (UglifyError uglifyError in result.Errors)
				Console.WriteLine($"{uglifyError.Message} at ({uglifyError.StartLine}:{uglifyError.StartColumn})");

			return Enumerable.Empty<string>();
		}

		string text = result.Code;
		return GetWords(text).Select(word => word.ToLower());
	}

	private static string? GetHeaderText(string html)
	{
		int headerTextIndex = html.IndexOf(HeadingOverrideRenderer.HeaderTextTag, StringComparison.Ordinal) + HeadingOverrideRenderer.HeaderTextTag.Length;

		if (headerTextIndex < 0)
			return null;
		int headerTextEnding = html.IndexOf("</span>", headerTextIndex, StringComparison.Ordinal);

		ReadOnlySpan<char> headerText = html.AsSpan()[headerTextIndex..headerTextEnding];

		if (!headerText.Contains('<'))
			return headerText.ToString();

		// Prune tags from header text.
		int length = headerText.Length;
		StringBuilder header = new StringBuilder(length);
		bool add = true;
		for (int i = 0; i < length; i++)
		{
			char c = headerText[i];
			switch (c)
			{
				case '<':
					add = false;
					continue;
				case '>':
					add = true;
					continue;
				default:
					if (!add)
						continue;
					break;
			}

			header.Append(c);
		}

		return header.ToString();
	}

	/// <summary>
	/// Does <see cref="query"/> end with any string contained in <see cref="endings"/>?
	/// </summary>
	private static bool EndsWithAny(string query, string[] endings)
	{
		foreach (string ending in endings)
		{
			if (query.EndsWith(ending))
				return true;
		}

		return false;
	}

	private static IEnumerable<string> GetWords(string text)
	{
		int current = 0, start = 0;
		for (; current < text.Length; current++)
		{
			if (IsWordCharacter(text[current]))
				continue;

			// Ensure the start of the word is valid
			while (start < current && !IsStartWordCharacter(text[start]))
				start++;

			if (start >= current - 1)
			{
				start++;
				continue;
			}

			// Ensure the end of the word is valid
			while (current - 1 > start && !IsEndWordCharacter(text[current - 1]))
				current--;

			Range range = start..current;
			(_, int length) = range.GetOffsetAndLength(text.Length);
			if (length >= 3)
				yield return text[range];

			start = current + 1;
		}

		if (start < text.Length - 1)
		{
			yield return text[start..^1];
		}

		bool IsWordCharacter(char c) => char.IsLetterOrDigit(c) || c == '\'';
		bool IsStartWordCharacter(char c) => char.IsLetterOrDigit(c);
		bool IsEndWordCharacter(char c) => char.IsLetterOrDigit(c);
	}
}