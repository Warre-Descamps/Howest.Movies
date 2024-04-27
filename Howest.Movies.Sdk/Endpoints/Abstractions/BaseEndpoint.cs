using System.Net.Http.Headers;
using System.Net.Http.Json;
using Howest.Movies.Sdk.Stores;
using Newtonsoft.Json;

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
    
    protected virtual HttpClient ApplyMiddleware(HttpClient client)
    {
        return client;
    }
    
    protected virtual async Task<HttpClient> ApplyAuthentication(HttpClient client)
    {
        var token = await _tokenStore.GetTokenAsync();
        if (token is null || string.IsNullOrWhiteSpace(token.AccessToken))
        {
            throw new InvalidOperationException("Token is not available or expired");
        }
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        return client;
    }
    
    protected virtual Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        return response.Content.ReadAsStringAsync().ContinueWith(content => JsonConvert.DeserializeObject<T>(content.Result));
    }

    private async Task<HttpClient> ConfigureClient(HttpClient client, bool authenticate = false)
    {
        client = ApplyMiddleware(client);
        
        if (authenticate)
        {
            client = await ApplyAuthentication(client);
        }

        return client;
    }

    protected Task<T?> GetAsync<T>(string url, bool authenticate = false)
    {
        return InvokeAsync(c => c.GetAsync(url),
            HandleResponse<T>,
            authenticate);
    }
    
    protected Task<TResult?> PostAsync<TResult>(string url, HttpContent content, bool authenticate = false)
    {
        return InvokeAsync(c => c.PostAsync(url, content),
            HandleResponse<TResult>,
            authenticate);
    }

    protected Task<TResult?> PostAsJsonAsync<TSource, TResult>(string url, TSource request, bool authenticate = false)
    {
        return InvokeAsync(c => c.PostAsJsonAsync(url, request),
            HandleResponse<TResult>,
            authenticate);
    }

    protected Task PostAsJsonAsync<T>(string url, T request, bool authenticate = false)
    {
        return InvokeAsync(c => c.PostAsJsonAsync(url, request),
            authenticate);
    }

    protected Task<TResult?> PutAsJsonAsync<TSource, TResult>(string url, TSource request, bool authenticate = false)
    {
        return InvokeAsync(c => c.PutAsJsonAsync(url, request),
            HandleResponse<TResult>,
            authenticate);
    }

    protected Task PutAsJsonAsync<T>(string url, T request, bool authenticate = false)
    {
        return InvokeAsync(c => c.PutAsJsonAsync(url, request),
            authenticate);
    }

    protected Task<TResult?> PatchAsync<TSource, TResult>(string url, TSource request, bool authenticate = false)
    {
        return InvokeAsync(c => c.PatchAsync(url, JsonContent.Create(request)),
            HandleResponse<TResult>,
            authenticate);
    }

    protected Task PatchAsync<T>(string url, T request, bool authenticate = false)
    {
        return InvokeAsync(c => c.PatchAsync(url, JsonContent.Create(request)),
            authenticate);
    }

    protected Task<T?> DeleteAsync<T>(string url, bool authenticate = false)
    {
        return InvokeAsync(c => c.DeleteAsync(url),
            HandleResponse<T>,
            authenticate);
    }

    protected Task DeleteAsync(string url, bool authenticate = false)
    {
        return InvokeAsync(c => c.DeleteAsync(url),
            authenticate);
    }

    protected async Task<T?> InvokeAsync<T>(Func<HttpClient, Task<HttpResponseMessage>> request,
        Func<HttpResponseMessage, HttpClient, Task<T?>>? handleResponse,
        bool authenticate = false)
    {
        ArgumentNullException.ThrowIfNull(request);

        var client = await ConfigureClient(HttpClient, authenticate);

        var response = await request(client);

        if (handleResponse is not null)
        {
            return await handleResponse(response, client);
        }
        
        return default;
    }
    
    protected virtual Task<T?> InvokeAsync<T>(Func<HttpClient, Task<HttpResponseMessage>> request,
        Func<HttpResponseMessage, Task<T?>>? handleResponse,
        bool authenticate = false)
    {
        return handleResponse is null
            ? InvokeAsync(request, (Func<HttpResponseMessage, HttpClient, Task<T?>>?)null, authenticate)
            : InvokeAsync(request, (response, _) => handleResponse.Invoke(response), authenticate);
    }

    protected Task InvokeAsync(Func<HttpClient, Task<HttpResponseMessage>> request, bool authenticate = false)
    {
        return InvokeAsync(request, (Func<HttpResponseMessage, Task<object?>>?)null, authenticate);
    }
}