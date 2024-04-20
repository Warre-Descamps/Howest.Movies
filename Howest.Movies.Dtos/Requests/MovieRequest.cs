namespace Howest.Movies.Dtos.Requests;

public class MovieRequest
{
    public required string Title { get; set; } = null!;
    public required string Description { get; set; } = null!;
    public required DateTime ReleaseDate { get; set; }
    public required string Director { get; set; } = null!;
    public string[] Genres { get; set; } = [];
}