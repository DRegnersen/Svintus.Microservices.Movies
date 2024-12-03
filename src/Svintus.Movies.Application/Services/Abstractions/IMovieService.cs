using Svintus.Movies.Application.Models;

namespace Svintus.Movies.Application.Services.Abstractions;

public interface IMovieService
{
    Task<MovieModel[]> GetRandomMoviesAsync(int? moviesNumber = null);

    Task RateMoviesAsync(long chatId, MovieRateModel[] rates);

    Task<MovieModel[]> GetRecommendedMoviesAsync(long chatId, int? moviesNumber = null);
}