using System.Net.Http.Headers;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

internal abstract class BaseEndpoint
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenStore _tokenStore;
    protected HttpClient HttpClient => _httpClientFactory.CreateClient(MoviesSdk.HttpClientName);

    public BaseEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
    {
        _httpClientFactory = httpClientFactory;
        _tokenStore = tokenStore;
    }
    
    protected async Task<HttpClient> GetAuthorizedClientAsync()
    {
        var client = HttpClient;
        var token = await _tokenStore.GetTokenAsync();
        if (token is null || string.IsNullOrWhiteSpace(token.AccessToken))
        {
            throw new InvalidOperationException("Token is not available or expired");
        }
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        return client;
    }
}