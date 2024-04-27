using System.Net;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

internal abstract class BaseAuthorizedEndpoint : BaseEndpoint
{
    private readonly ITokenRefresh<ServiceResult> _tokenRefresh;

    protected BaseAuthorizedEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore, ITokenRefresh<ServiceResult> tokenRefresh) : base(httpClientFactory, tokenStore)
    {
        _tokenRefresh = tokenRefresh;
    }

    protected override Task<T?> InvokeAsync<T>(Func<HttpClient, Task<HttpResponseMessage>> request,
        Func<HttpResponseMessage, Task<T?>>? handleResponse,
        bool authenticate = false) where T : default
    {
        
        
        return base.InvokeAsync(request, RefreshingHanldeResponse, authenticate);

        async Task<T?> RefreshingHanldeResponse(HttpResponseMessage response, HttpClient client)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _tokenRefresh.RefreshAsync();
                client = await ApplyAuthentication(client);
                response = await request(client);
            }

            if (handleResponse is not null)
            {
                return await handleResponse(response);
            }

            return default;
        }
    }
}