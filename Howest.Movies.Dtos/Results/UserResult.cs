namespace Howest.Movies.Dtos.Results;

public class UserResult
{
    public Guid Id { get; set; }
    public required string UserName { get; set; } = null!;
}