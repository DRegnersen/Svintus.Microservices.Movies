using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Svintus.Movies.DataAccess.Models;
using Svintus.Movies.DataAccess.Models.Options;
using Svintus.Movies.DataAccess.Services.Abstractions;

namespace Svintus.Movies.DataAccess.Services;

internal sealed class MovieRepository(IMongoClient client, IOptions<MongoRepositoryOptions> options) : IMovieRepository
{
    private readonly IMongoCollection<Movie> _collection = client.GetDatabase(options.Value.DatabaseName).GetCollection<Movie>("Movies");

    public async Task<List<Movie>> GetMoviesAsync(long[] movieIds)
    {
        var filter = Builders<Movie>.Filter.In(movie => movie.MovieId, movieIds);

        return await _collection.Find(filter).ToListAsync();
    }
    
    public async Task<List<Movie>> GetMoviesAsync(int? moviesNumber = null)
    {
        if (moviesNumber.HasValue)
        {
            return await _collection
                .Aggregate()
                .Sample(moviesNumber.Value)
                .ToListAsync();
        }

        return await _collection.Find(FilterDefinition<Movie>.Empty).ToListAsync();
    }
}