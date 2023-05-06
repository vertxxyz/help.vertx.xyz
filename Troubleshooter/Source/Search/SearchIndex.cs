using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Troubleshooter.Search;

public static class SearchIndex
{
	public static string GetJsonFilePath(Arguments arguments) => Path.Combine(Path.Combine(arguments.Path!, "Json"), "search-index.json");

	public static async Task Generate(Arguments arguments, IEnumerable<string> paths)
	{
		List<string> pathsIn = paths.ToList();
		pathsIn.RemoveAll(f => !f.EndsWith(".html"));
		(IList<string> filePaths, IList<string> fileHeaders, ImmutableSortedDictionary<string, Dictionary<int, int>> sortedWordsToFileIndexAndCount) =
			await SearchGatherer.GenerateSearchResult(arguments.HtmlOutputDirectory!, pathsIn);
		await Generate(arguments, sortedWordsToFileIndexAndCount, filePaths, fileHeaders);
	}
	
	public static async Task Generate(Arguments arguments)
	{
		// Gather files.
		(IList<string> filePaths, IList<string> fileHeaders, ImmutableSortedDictionary<string, Dictionary<int, int>> sortedWordsToFileIndexAndCount) =
			await SearchGatherer.GenerateSearchResult(arguments.HtmlOutputDirectory!);
		await Generate(arguments, sortedWordsToFileIndexAndCount, filePaths, fileHeaders);
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