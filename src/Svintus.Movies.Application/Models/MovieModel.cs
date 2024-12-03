namespace Svintus.Movies.Application.Models;

public sealed class MovieModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string[] Genres { get; set; } = [];
}