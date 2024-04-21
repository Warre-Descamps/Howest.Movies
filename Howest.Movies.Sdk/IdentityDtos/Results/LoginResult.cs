namespace Howest.Movies.Sdk.IdentityDtos.Results;

public class LoginResult
{
    public required string TokenType { get; set; } = null!;
    public required string AccessToken { get; set; } = null!;
    public required int ExpiresIn { get; set; }
    public required string RefreshToken { get; set; } = null!;
}