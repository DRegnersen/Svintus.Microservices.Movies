using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Svintus.Movies.DataAccess.Models;
using Svintus.Movies.DataAccess.Models.Options;
using Svintus.Movies.DataAccess.Services.Abstractions;

namespace Svintus.Movies.DataAccess.Services;

internal sealed class RatingRepository(IMongoClient client, IOptions<MongoRepositoryOptions> options) : IRatingRepository
{
    private const long FictitiousUsersNumber = 6040;
    
    private readonly IMongoCollection<UserRating> _collection = client.GetDatabase(options.Value.DatabaseName).GetCollection<UserRating>("Ratings");

    public async Task<UserRating?> FindRatingAsync(long chatId)
    {
        return await _collection.Find(rating => rating.ChatId == chatId).FirstOrDefaultAsync();
    }
    
    public async Task<UserRating> CreateRatingAsync(long chatId, MovieRate[] rates)
    {
        var userId = await GenerateUserIdAsync();
        var userRating = new UserRating { ChatId = chatId, UserId = userId, Rates = rates };
        
        await _collection.InsertOneAsync(userRating);
        return userRating;
    }
    
    public async Task<UserRating> AddRatesAsync(long chatId, MovieRate[] rates)
    {
        var update = Builders<UserRating>.Update.PushEach(p => p.Rates, rates);

        return await _collection.FindOneAndUpdateAsync(
            rating => rating.ChatId == chatId,
            update,
            new FindOneAndUpdateOptions<UserRating>
            {
                ReturnDocument = ReturnDocument.After
            }
        );
    }
    
    private async Task<long> GenerateUserIdAsync()
    {
        var actualUsersNumber = await _collection.CountDocumentsAsync(FilterDefinition<UserRating>.Empty);
        return FictitiousUsersNumber + actualUsersNumber;
    }
}