using System.Text.Json.Serialization;

namespace Svintus.Movies.Integrations.RecommModel.Models.Dto.Serialization;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(CreateUserRequestDto))]
[JsonSerializable(typeof(UpdateUserRequestDto))]
[JsonSerializable(typeof(RecommsResponseDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
internal sealed partial class RecommModelJsonContext : JsonSerializerContext
{
}