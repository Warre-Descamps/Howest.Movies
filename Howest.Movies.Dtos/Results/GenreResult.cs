namespace Howest.Movies.Dtos.Results;

public class GenreResult
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = null!;
}