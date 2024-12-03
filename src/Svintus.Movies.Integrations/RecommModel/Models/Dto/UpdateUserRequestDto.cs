namespace Svintus.Movies.Integrations.RecommModel.Models.Dto;

internal sealed class UpdateUserRequestDto
{
    public UserMovieRateDto[] Rates { get; set; } = [];
}