namespace Howest.Movies.Models;

public class Genre
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = null!;

    public ICollection<MovieGenre> Movies { get; set; } = new List<MovieGenre>();
}