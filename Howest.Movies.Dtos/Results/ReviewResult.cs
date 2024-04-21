namespace Howest.Movies.Dtos.Results;

public class ReviewResult
{
    public Guid Id { get; set; }
    public required Guid MovieId { get; set; }
    public required byte Rating { get; set; }
    public required string Comment { get; set; }
    public required DateTime ReviewDate { get; set; }
}