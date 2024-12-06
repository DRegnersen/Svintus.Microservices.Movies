using Microsoft.Extensions.Options;
using Svintus.Movies.Application.Models;
using Svintus.Movies.Application.Models.Options;
using Svintus.Movies.Application.Models.Results;
using Svintus.Movies.Application.Services.Abstractions;
using Svintus.Movies.DataAccess.Models;
using Svintus.Movies.DataAccess.Services.Abstractions;
using Svintus.Movies.Integrations.RecommModel.Client.Abstractions;
using Svintus.Movies.Integrations.RecommModel.Models;

namespace Svintus.Movies.Application.Services;

internal sealed class MovieService(
    IMovieRepository movieRepository,
    IRatingRepository ratingRepository,
    IRecommModelClient modelClient,
    IOptions<MovieServiceOptions> options) : IMovieService
{
    private readonly MovieServiceOptions _options = options.Value;

    public async Task<MovieModel[]> GetRandomMoviesAsync(int? moviesNumber = null)
    {
        List<Movie>? movies;

        if (moviesNumber.HasValue)
        {
            movies = await movieRepository.GetMoviesAsync(moviesNumber.Value);
        }
        else
        {
            movies = await movieRepository.GetMoviesAsync(_options.DefaultRandomMoviesNumber);
        }

        return movies.Select(Mapper.Map).ToArray();
    }

    public async Task RateMoviesAsync(long chatId, MovieRateModel[] rates)
    {
        var foundRating = await ratingRepository.FindRatingAsync(chatId);

        if (foundRating is null)
        {
            var createdRating = await ratingRepository.CreateRatingAsync(chatId, rates.Select(Mapper.Map).ToArray());

            await modelClient.CreateUserAsync(createdRating.UserId, createdRating.Rates.Select(Mapper.Map).ToArray());
        }
        else
        {
            var updatedRating = await ratingRepository.AddRatesAsync(chatId, rates.Select(Mapper.Map).ToArray());

            await modelClient.UpdateUserAsync(updatedRating.UserId, updatedRating.Rates.Select(Mapper.Map).ToArray());
        }
    }

    public async Task<Result<MovieModel[], Error>> GetRecommendedMoviesAsync(long chatId, int? moviesNumber = null)
    {
        var rating = await ratingRepository.FindRatingAsync(chatId);
        if (rating is null)
        {
            return new Error(ResultCode.ChatIdNotFound, $"Rating with chat id {chatId} was not found");
        }

        MovieRecommendation[]? recomms;

        if (moviesNumber.HasValue)
        {
            recomms = await modelClient.GetRecommendationsAsync(rating.UserId, moviesNumber.Value);
        }
        else
        {
            recomms = await modelClient.GetRecommendationsAsync(rating.UserId, _options.DefaultRecommendedMoviesNumber);
        }

        var movies = await movieRepository.GetMoviesAsync(recomms.Select(r => r.MovieId).ToArray());
        return movies.Select(Mapper.Map).ToArray();
    }
}

#region Mappings

file static class Mapper
{
    public static MovieModel Map(Movie movie) => new()
    {
        Id = movie.MovieId,
        Title = movie.Title,
        Genres = movie.Genres
    };

    public static MovieRate Map(MovieRateModel rateModel) => new()
    {
        MovieId = rateModel.MovieId,
        Rate = rateModel.Rate
    };

    public static UserMovieRate Map(MovieRate rateModel) => new(rateModel.MovieId, rateModel.Rate);
}

#endregion