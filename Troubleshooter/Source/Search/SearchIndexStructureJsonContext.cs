using System.Text.Json.Serialization;

namespace Troubleshooter.Search;

[JsonSourceGenerationOptions(
	GenerationMode = JsonSourceGenerationMode.Serialization,
	PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(SearchIndexStructure))]
internal sealed partial class SearchIndexStructureJsonContext : JsonSerializerContext;
