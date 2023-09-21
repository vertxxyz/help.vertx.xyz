using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Troubleshooter.Issues;

public static class SourceIndex
{
	public static void GeneratePageSourceLookup(Arguments arguments, PageResourcesLookup pageResources)
	{
		// Gather file paths.
		int trimIndex = arguments.Root.Length;
		Dictionary<string, string> sourceLookup = pageResources
			.Where(pair =>
				pair.Value is { Location: ResourceLocation.Site, Type: ResourceType.Markdown }
			).ToDictionary(pair => pair.Value.OutputLink.ToOutputPath(), pair => pair.Key[trimIndex..].ToOutputPath());

		// Serialize Json.
		SourceIndexStructure structure = new(sourceLookup);
		string json = JsonSerializer.Serialize(structure, SourceIndexStructureJsonContext.Default.SourceIndexStructure);

		IOUtility.CreateFileIfDifferent(Path.Combine(arguments.JsonOutputDirectory!, "source-index.json"), json, IOUtility.RecordType.Normal);
	}
}
