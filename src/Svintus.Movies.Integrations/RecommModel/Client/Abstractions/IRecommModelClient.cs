using Svintus.Movies.Integrations.RecommModel.Models;

namespace Svintus.Movies.Integrations.RecommModel.Client.Abstractions;

public interface IRecommModelClient
{
    Task CreateUserAsync(long userId, UserMovieRate[] rates);

    Task UpdateUserAsync(long userId, UserMovieRate[] rates);

    Task<MovieRecommendation[]> GetRecommendationsAsync(long userId, int? recommsNumber = null);
}