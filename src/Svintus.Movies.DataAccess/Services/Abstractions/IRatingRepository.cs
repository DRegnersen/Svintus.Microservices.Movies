using Svintus.Movies.DataAccess.Models;

namespace Svintus.Movies.DataAccess.Services.Abstractions;

public interface IRatingRepository
{
    Task<UserRating?> FindRatingAsync(long chatId);

    Task<UserRating> CreateRatingAsync(long chatId, MovieRate[] rates);

    Task<UserRating> AddRatesAsync(long chatId, MovieRate[] rates);
}