using Grpc.Core;
using Svintus.Movies.Application.Models;
using Svintus.Movies.Application.Services.Abstractions;

namespace Svintus.Microservices.Movies.Services;

public sealed class MovieGrpcService(IMovieService movieService) : MovieService.MovieServiceBase
{
    public override async Task<GetRandomMoviesResponse> GetRandomMovies(GetRandomMoviesRequest request, ServerCallContext context)
    {
        var moviesNumber = request.MoviesNumber is 0 or > int.MaxValue ? null : (int?)request.MoviesNumber;

        var movies = await movieService.GetRandomMoviesAsync(moviesNumber);

        var response = new GetRandomMoviesResponse();
        response.Movies.Add(movies.Select(Mapper.Map));

        return response;
    }

    public override async Task<RateMoviesResponse> RateMovies(RateMoviesRequest request, ServerCallContext context)
    {
        await movieService.RateMoviesAsync(request.ChatId, request.Rates.Select(Mapper.Map).ToArray());

        return new RateMoviesResponse { Success = true };
    }

    public override async Task<GetRecommendedMoviesResponse> GetRecommendedMovies(GetRecommendedMoviesRequest request, ServerCallContext context)
    {
        var moviesNumber = request.MoviesNumber is 0 or > int.MaxValue ? null : (int?)request.MoviesNumber;

        var movies = await movieService.GetRecommendedMoviesAsync(request.ChatId, moviesNumber);

        var response = new GetRecommendedMoviesResponse();
        response.Movies.Add(movies.Select(Mapper.Map));

        return response;
    }
}

#region Mappings

file static class Mapper
{
    public static Movie Map(MovieModel movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title
    };

    public static MovieRateModel Map(MovieRate rateModel)
    {
        if (rateModel.Rate > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(rateModel.Rate));
        }

        return new MovieRateModel(rateModel.MovieId, (int)rateModel.Rate);
    }
}

#endregion