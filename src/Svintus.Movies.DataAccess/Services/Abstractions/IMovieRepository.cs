using Svintus.Movies.DataAccess.Models;

namespace Svintus.Movies.DataAccess.Services.Abstractions;

public interface IMovieRepository
{
    Task<List<Movie>> GetMoviesAsync(long[] movieIds);
    
    Task<List<Movie>> GetMoviesAsync(int? moviesNumber = null);
}