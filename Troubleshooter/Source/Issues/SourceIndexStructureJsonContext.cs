using System.Text.Json.Serialization;

namespace Troubleshooter.Issues;

[JsonSourceGenerationOptions(
	GenerationMode = JsonSourceGenerationMode.Serialization,
	PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(SourceIndexStructure))]
internal partial class SourceIndexStructureJsonContext : JsonSerializerContext { }