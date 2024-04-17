namespace Howest.Movies.Models;

public class MovieGenre
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = null!;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}