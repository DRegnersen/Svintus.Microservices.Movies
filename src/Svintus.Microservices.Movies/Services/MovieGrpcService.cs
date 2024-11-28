using Grpc.Core;

namespace Svintus.Microservices.Movies.Services;

public sealed class MovieGrpcService : MovieService.MovieServiceBase
{
    public override Task<GetRandomMoviesResponse> GetRandomMovies(GetRandomMoviesRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
    
    public override Task<RateMoviesResponse> RateMovies(RateMoviesRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}