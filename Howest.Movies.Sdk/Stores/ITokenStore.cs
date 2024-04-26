using Howest.Movies.Sdk.IdentityDtos.Results;

namespace Howest.Movies.Sdk.Stores;

public interface ITokenStore
{
    Task<LoginResult?> GetTokenAsync();
    Task SetTokenAsync(LoginResult token);
    Task RemoveTokenAsync();
}