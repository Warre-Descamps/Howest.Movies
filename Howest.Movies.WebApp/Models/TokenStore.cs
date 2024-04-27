using Blazored.LocalStorage;
using Howest.Movies.Sdk.IdentityDtos.Results;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.WebApp.Models;

public class TokenStore: ITokenStore
{
    private readonly ILocalStorageService _localStorageService;

    public TokenStore(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<LoginResult?> GetTokenAsync()
    {
        if (await _localStorageService.ContainKeyAsync("login"))
            return await _localStorageService.GetItemAsync<LoginResult>("login");
        return null;
    }

    public async Task SetTokenAsync(LoginResult token)
    {
        await _localStorageService.SetItemAsync("login", token);
    }

    public async Task RemoveTokenAsync()
    {
        await _localStorageService.RemoveItemAsync("login");
    }
}