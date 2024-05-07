using System.Net;
using System.Net.Http.Json;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Extensions;
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

    public IdentityEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore) : base(httpClientFactory, tokenStore)
    {
        _tokenStore = tokenStore;
    }

    private static async Task<ServiceResult> ReadErrors(HttpResponseMessage response)
    {
        var result = new ServiceResult();
        if (response.StatusCode == HttpStatusCode.OK) return result;
        
        var validationResult = await response.Content.ReadFromJsonAsync<ValidationError>();
        if (validationResult?.Status == 401)
            result = result.Unauthorized();
        result.Messages.AddRange(validationResult?.Errors
            .Select(pair => new ServiceMessage(pair.Key, pair.Value.First(), MessageType.Error))
            .ToList() ?? []);

        return result;
    }
    
    private static async Task<ServiceResult<T>> ReadErrors<T>(HttpResponseMessage response) where T : class
    {
        var result = await ReadErrors(response);
        if (result.IsSuccess)
        {
            var data = await response.Content.ReadFromJsonAsync<T>();
            return new ServiceResult<T> { Data = data };
        }
        
        return new ServiceResult<T>(result.Messages.ToArray());
    }
    
    public async Task<ServiceResult> RefreshAsync(bool fromBackground)
    {
        var token = await _tokenStore.GetTokenAsync();
        if (token is null)
            return new ServiceResult<LoginResult>(new ServiceMessage("Refresh", "No refresh token found.", MessageType.Error));
        
        var response = await HttpClient.PostAsJsonAsync("/api/identity/refresh", new { token.RefreshToken });
        
        var result = await ReadErrors<LoginResult>(response);
        if (result.IsSuccess)
        {
            await _tokenStore.SetTokenAsync(result.Data!);
            if (!fromBackground && OnLogin is not null)
                await OnLogin.Invoke();
        }
        else
        {
            result.Messages.Add(new ServiceMessage("Refresh", "Invalid refresh token.", MessageType.Error));
        }

        return result;
    }

    public async Task<ServiceResult> RegisterAsync(Request request)
    {
        var response = await HttpClient.PostAsJsonAsync("/api/identity/register", request);
        
        return await ReadErrors(response);
    }

    public async Task<ServiceResult<LoginResult>> LoginAsync(Request request)
    {
        var response = await HttpClient.PostAsJsonAsync("/api/identity/login", request);
        
        var result = await ReadErrors<LoginResult>(response);
        if (result.IsSuccess)
        {
            await _tokenStore.SetTokenAsync(result.Data!);
            if (OnLogin is not null)
                await OnLogin.Invoke();
        }
        else
        {
            result.Messages = [new ServiceMessage("login", "Invalid credentials.", MessageType.Error)];
        }
        
        return result;
    }

    public async Task<ServiceResult<UserInfoResult>> TryGetUserAsync()
    {
        try
        {
            var client = await ApplyAuthentication(HttpClient);
            
            var response = await client.GetAsync("/api/identity/manage/info");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await ReadErrors<UserInfoResult>(response);
                case HttpStatusCode.Unauthorized:
                    await RefreshAsync(true);
                    client = await ApplyAuthentication(HttpClient);
                    response = await client.GetAsync("/api/identity/manage/info");
                    if (response.StatusCode == HttpStatusCode.OK)
                        return new ServiceResult<UserInfoResult>
                        {
                            Data = await response.Content.ReadFromJsonAsync<UserInfoResult>()
                        };
                    break;
            }
            return await ReadErrors<UserInfoResult>(response);
        }
        catch
        {
            return new ServiceResult<UserInfoResult>().Unauthorized();
        }
    }

    public async Task<ServiceResult> LogoutAsync()
    {
        await _tokenStore.RemoveTokenAsync();
        if (OnLogout is not null)
            await OnLogout.Invoke();
        return new ServiceResult();
    }

    public Task<ServiceResult> RefreshAsync()
    {
        return RefreshAsync(false);
    }
}