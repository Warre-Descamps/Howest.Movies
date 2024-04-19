namespace Howest.Movies.Dtos.Results;

public class MovieDetailResult : MovieResult
{
    public required string Description { get; set; } = null!;
    public required DateTime ReleaseDate { get; set; }
    public required string Director { get; set; } = null!;
    public UserResult? AddedBy { get; set; }
}