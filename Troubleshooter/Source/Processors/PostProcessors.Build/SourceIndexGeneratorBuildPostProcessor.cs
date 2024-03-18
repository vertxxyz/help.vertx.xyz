using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Troubleshooter.Issues;

namespace Troubleshooter;

/// <summary>
/// Generates the source-index.json lookup, used when reporting issues with pages.
/// </summary>
[UsedImplicitly]
public sealed class SourceIndexGeneratorBuildPostProcessor : IBuildPostProcessor
{
	public int Order => 9999;

	public void Process(Arguments arguments, PageResourcesLookup resources, Site site, ILogger logger)
	{
		// Gather file paths.
		int trimIndex = arguments.Root.Length;
		Dictionary<string, string> sourceLookup = resources
			.Where(pair =>
				pair.Value is { Location: ResourceLocation.Site, Type: ResourceType.Markdown, IsSidebar: false }
			).ToDictionary(pair => pair.Value.OutputLink.ToOutputPath(), pair => pair.Key[trimIndex..].ToOutputPath());

		string json = JsonSerializer.Serialize(new SourceIndexStructure(sourceLookup), SourceIndexStructureJsonContext.Default.SourceIndexStructure);

		IOUtility.CreateFileIfDifferent(Path.Combine(arguments.JsonOutputDirectory!, "source-index.json"), json, IOUtility.RecordType.Normal);
	}
}
