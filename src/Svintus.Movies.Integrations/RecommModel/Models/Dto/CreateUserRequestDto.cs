namespace Svintus.Movies.Integrations.RecommModel.Models.Dto;

internal sealed class CreateUserRequestDto
{
    public UserMovieRateDto[] Rates { get; set; } = [];
}