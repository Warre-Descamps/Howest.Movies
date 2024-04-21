namespace Howest.Movies.Dtos.Results;

public class MovieResult
{
    public Guid Id { get; set; }
    public required string Title { get; set; } = null!;
    public required string Poster { get; set; } = null!;
}