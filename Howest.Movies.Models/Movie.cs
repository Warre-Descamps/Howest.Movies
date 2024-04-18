namespace Howest.Movies.Models;

public class Movie
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; } = null!;
    public required DateTime ReleaseDate { get; set; }
    public required string Director { get; set; } = null!;
    public required string Description { get; set; } = null!;
    
    public required Guid AddedById { get; set; }
    public User? AddedByUser { get; set; }
    public ICollection<MovieGenre> Genres { get; set; } = new List<MovieGenre>();
    public ICollection<MovieReview> Reviews { get; set; } = new List<MovieReview>();
}