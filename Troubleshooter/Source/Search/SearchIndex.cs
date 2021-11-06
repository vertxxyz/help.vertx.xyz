using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Troubleshooter.Search
{
	public static class SearchIndex
	{
		public static string GetJsonFilePath(Arguments arguments) => Path.Combine(Path.Combine(arguments.Path, "Json"), "search-index.json");
		
		public static async Task Generate(Arguments arguments)
		{
			// Gather files.
			(IList<string> filePaths, IList<string> fileHeaders, ImmutableSortedDictionary<string, Dictionary<int, int>> sortedWordsToFileIndexAndCount) =
				await SearchGatherer.GenerateSearchResult(arguments.HtmlOutputDirectory);

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
			SearchIndexStructure index = new SearchIndexStructure(filePaths, fileHeaders, termsToIndices, SearchCommon.CommonValues);
			string json = JsonSerializer.Serialize(index, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			}); // TODO use SearchIndexStructureJsonContext when Rider supports it.

			// Write index file.
			string jsonDirectory = Path.Combine(arguments.Path, "Json");
			if (!Directory.Exists(jsonDirectory))
				Directory.CreateDirectory(jsonDirectory);

			await using var file = File.CreateText(Path.Combine(jsonDirectory, "search-index.json"));
			await file.WriteAsync(json);
			Console.WriteLine("Generated search index.");
		}
	}
}