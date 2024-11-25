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

public static partial class SearchGatherer
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

	[GeneratedRegex("""<pre(?: class="\w*?")>.+?</pre>""", RegexOptions.Singleline)]
	private static partial Regex PreRegex { get; }

	public static async Task<Result> GenerateSearchResult(string rootDirectory, IEnumerable<string> files)
	{
		Dictionary<string, int> fileNameToIndex = new();
		List<string> filePaths = [];
		foreach (string s in files)
		{
			if (EndsWithAny(s, SearchCommon.ExcludedFileEndings)) continue;
			string localPath = s[(rootDirectory.Length + 1)..];
			fileNameToIndex[s] = filePaths.Count;
			filePaths.Add($"/{localPath.Replace('\\', '/')}");
		}

		ConcurrentDictionary<int, string> headerText = new();
		ConcurrentDictionary<string, Dictionary<int, int>> wordsToFileIndexAndCount = new();
		await Parallel.ForEachAsync(fileNameToIndex, async (pair, _) =>
		{
			(string file, int index) = pair;
			if (EndsWithAny(file, SearchCommon.ExcludedFileEndings)) return;
			try
			{
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
			}
			catch (Exception)
			{
				Console.WriteLine($"{file} failed to parse for {nameof(GenerateSearchResult)}.");
				throw;
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

		string? headerText = GetHeaderText(html, path);
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

		// Remove extremely common headers from search results
		html = html.Replace(">Description<", "><").Replace(">Resolution<", "><").Replace(">Implementation<", "><");

		// Without this wrapping div Uglify fails to parse many forms of HTML.
		UglifyResult result = Uglify.HtmlToText($"<div>{html}</div>", sourceFileName: path);
		if (result.HasErrors)
		{
			Console.WriteLine($"Error while parsing \"{path}\" to text.");
			foreach (UglifyError uglifyError in result.Errors)
				Console.WriteLine($"{uglifyError.Message} at ({uglifyError.StartLine}:{uglifyError.StartColumn})");

			return [];
		}

		string text = result.Code;
		return GetWords(text).Select(word => word.ToLower());
	}

	private static string? GetHeaderText(string html, string context)
	{
		int headerTextIndex = html.IndexOf(HeadingOverrideRenderer.HeaderTextTag, StringComparison.Ordinal);

		if (headerTextIndex < 0)
			return null;
		headerTextIndex += HeadingOverrideRenderer.HeaderTextTag.Length;
		int headerTextEnding = html.IndexOf("</span>", headerTextIndex, StringComparison.Ordinal);

		if (headerTextEnding < 0)
			throw new BuildException($"Header text is not properly terminated in \"{context}\".");

		ReadOnlySpan<char> headerText = html.AsSpan()[headerTextIndex..headerTextEnding];

		if (!headerText.Contains('<'))
			return headerText.ToString();

		// Prune tags from header text.
		int length = headerText.Length;
		StringBuilder header = new(length);
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

		yield break;

		static bool IsWordCharacter(char c) => char.IsLetterOrDigit(c) || c == '\'';
		static bool IsStartWordCharacter(char c) => char.IsLetterOrDigit(c);
		static bool IsEndWordCharacter(char c) => char.IsLetterOrDigit(c);
	}
}
