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
			).ToDictionary(pair => pair.Value.OutputLinkPath!, pair => pair.Key[trimIndex..].Replace('\\', '/'));

		// Serialize Json.
		SourceIndexStructure structure = new(sourceLookup);
		string json = JsonSerializer.Serialize(structure, SourceIndexStructureJsonContext.Default.SourceIndexStructure);

		IOUtility.CreateFileIfDifferent(Path.Combine(arguments.JsonOutputDirectory!, "source-index.json"), json);
	}
}