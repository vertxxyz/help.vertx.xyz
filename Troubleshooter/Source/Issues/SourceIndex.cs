using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Troubleshooter.Issues;

public static class SourceIndex
{
	public static void GeneratePageSourceLookup(Arguments arguments, PageResources pageResources)
	{
		// Gather file paths.
		int trimIndex = arguments.TroubleshooterRoot.Length;
		Dictionary<string, string> sourceLookup = pageResources
			.Where(pair =>
				pair.Value.Location == ResourceLocation.Site &&
				pair.Value.Type == ResourceType.Markdown
			).ToDictionary(pair => pair.Value.OutputLinkPath, pair => pair.Key[trimIndex..].Replace('\\', '/'));

		// Serialize Json.
		SourceIndexStructure structure = new SourceIndexStructure(sourceLookup);
		string json = JsonSerializer.Serialize(structure, SourceIndexStructureJsonContext.Default.SourceIndexStructure);

		IOUtility.CreateFileIfDifferent(Path.Combine(arguments.JsonOutputDirectory, "source-index.json"), json);
	}
}