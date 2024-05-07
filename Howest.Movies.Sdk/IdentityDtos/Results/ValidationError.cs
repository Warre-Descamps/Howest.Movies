namespace Howest.Movies.Sdk.IdentityDtos.Results;

public class ValidationError
{
    public required string Type { get; set; } = null!;
    public required string Title { get; set; } = null!;
    public required int Status { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; } = [];
}