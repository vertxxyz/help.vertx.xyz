using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Troubleshooter.Search;

public static class SearchIndex
{
	public static string GetJsonFilePath(Arguments arguments) => Path.Combine(Path.Combine(arguments.Path, "Json"), "search-index.json");

	public static async Task<bool> Generate(Arguments arguments, ReadOnlyDictionary<string, IOUtility.RecordType> paths)
	{
		try
		{
			List<string> pathsIn = paths
				// Only process normal files as a part of the search index.
				.Where(kvp =>
				{
					return kvp.Value switch
					{
						IOUtility.RecordType.Normal => true,
						IOUtility.RecordType.Index => false,
						IOUtility.RecordType.Duplicate => false,
						_ => true
					};
				})
				.Select(kvp => kvp.Key)
				// Only html files can be searched
				.Where(f => f.EndsWith(".html"))
				.ToList();
			(IList<string> filePaths, IList<string> fileHeaders, ImmutableSortedDictionary<string, Dictionary<int, int>> sortedWordsToFileIndexAndCount) =
				await SearchGatherer.GenerateSearchResult(arguments.Path, pathsIn);
			await Generate(arguments, sortedWordsToFileIndexAndCount, filePaths, fileHeaders);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return false;
		}

		return true;
	}

	private static async Task Generate(Arguments arguments, ImmutableSortedDictionary<string, Dictionary<int, int>> sortedWordsToFileIndexAndCount, IList<string> filePaths, IList<string> fileHeaders)
	{
		// Create words to indices lookup
		var termsToIndices = new Dictionary<string, int[]>();
		foreach (KeyValuePair<string, Dictionary<int, int>> pair in sortedWordsToFileIndexAndCount)
		{
			(string term, Dictionary<int, int> fileIndexToCount) = pair;
			int[] sortedFileIndices = fileIndexToCount
				.OrderBy(indexAndCount => indexAndCount.Value)
				.ThenBy(indexAndCount => filePaths[indexAndCount.Key])
				.Select(indexAndCount => indexAndCount.Key)
				.ToArray();
			termsToIndices.Add(term, sortedFileIndices);
		}

		// Serialize Json.
		SearchIndexStructure index = new(filePaths, fileHeaders, termsToIndices, SearchCommon.CommonValues);
		string json = JsonSerializer.Serialize(index, SearchIndexStructureJsonContext.Default.SearchIndexStructure);

		// Write index file.
		await using var file = File.CreateText(Path.Combine(arguments.JsonOutputDirectory!, "search-index.json"));
		await file.WriteAsync(json);
	}
}
