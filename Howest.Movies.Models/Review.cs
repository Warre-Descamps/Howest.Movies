namespace Howest.Movies.Models;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required byte Rating { get; set; }
    public required string Comment { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    
    public required Guid MovieId { get; set; }
    public Movie? Movie { get; set; }
    public required Guid ReviewerId { get; set; }
    public User? Reviewer { get; set; }
}