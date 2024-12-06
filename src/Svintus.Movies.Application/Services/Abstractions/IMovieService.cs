using Svintus.Movies.Application.Models;
using Svintus.Movies.Application.Models.Results;

namespace Svintus.Movies.Application.Services.Abstractions;

public interface IMovieService
{
    Task<MovieModel[]> GetRandomMoviesAsync(int? moviesNumber = null);

    Task RateMoviesAsync(long chatId, MovieRateModel[] rates);

    Task<Result<MovieModel[], Error>> GetRecommendedMoviesAsync(long chatId, int? moviesNumber = null);
}