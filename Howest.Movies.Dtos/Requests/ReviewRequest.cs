namespace Howest.Movies.Dtos.Requests;

public class ReviewRequest
{
    public required byte Rating { get; set; }
    public required string Comment { get; set; }
}