using System.Net;
using System.Net.Http.Json;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.IdentityDtos;
using Howest.Movies.Sdk.IdentityDtos.Results;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk.Endpoints;

internal class IdentityEndpoint : BaseEndpoint, IIdentityEndpoint
{
    private readonly ITokenStore _tokenStore;

    public event Func<Task>? OnLogin;
    public event Func<Task>? OnLogout;

    public IdentityEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore) : base(httpClientFactory)
    {
        _tokenStore = tokenStore;
    }
    
    private static T ReadErrors<T>(HttpResponseMessage response) where T : ServiceResult, new()
    {
        var result = new T();
        if (response.StatusCode == HttpStatusCode.OK) return result;
        
        var errors = response.Content.ReadFromJsonAsync<ValidationError>();
        result.Messages = errors.Result?.Errors
            .Select(pair => new ServiceMessage(pair.Key, pair.Value.First(), MessageType.Error))
            .ToList() ?? [];

        return result;
    }

    public async Task<ServiceResult> RegisterAsync(Request request)
    {
        var response = await HttpClient.PostAsJsonAsync("/api/identity/register", request);
        
        var result = ReadErrors<ServiceResult>(response);
        return result;
    }

    public async Task<ServiceResult<LoginResult>> LoginAsync(Request request)
    {
        var response = await HttpClient.PostAsJsonAsync("/api/identity/login", request);
        
        var result = ReadErrors<ServiceResult<LoginResult>>(response);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
            result.Data = loginResult;
            
            await _tokenStore.SetTokenAsync(loginResult!);
            if (OnLogin is not null)
                await OnLogin.Invoke();
        }
        else
        {
            result.Messages.Add(new ServiceMessage("login", "Invalid credentials", MessageType.Error));
        }
        
        return result;
    }

    public async Task<ServiceResult> LogoutAsync()
    {
        await _tokenStore.RemoveTokenAsync();
        if (OnLogout is not null)
            await OnLogout.Invoke();
        return new ServiceResult();
    }

    public async Task<ServiceResult<LoginResult>> RefreshAsync(string refreshToken)
    {
        var response = await HttpClient.PostAsJsonAsync("/api/identity/refresh", new { refreshToken });
        
        var result = ReadErrors<ServiceResult<LoginResult>>(response);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
            result.Data = loginResult;
        }
        else
        {
            result.Messages.Add(new ServiceMessage("refresh", "Invalid refresh token", MessageType.Error));
        }

        return result;
    }
}