using MongoDB.Bson.Serialization.Attributes;

namespace Svintus.Movies.DataAccess.Models;

public sealed class Movie
{
    [BsonId]
    public int MovieId { get; set; }
    public string Title { get; set; } = default!;
    
    [BsonElement("Genres")]
    public string GenresString { get; set; } = default!;
    
    public string[] Genres => GenresString.Split("|");
}