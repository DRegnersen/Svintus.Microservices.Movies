using MongoDB.Bson.Serialization.Attributes;

namespace Svintus.Movies.DataAccess.Models;

public sealed class UserRating
{
    [BsonId]
    public long ChatId { get; set; }
    public long UserId { get; set; }
    public MovieRate[] Rates { get; set; } = [];
}

public sealed class MovieRate
{
    public long MovieId { get; set; }
    public int Rate { get; set; }
}