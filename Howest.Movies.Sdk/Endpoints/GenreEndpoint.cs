using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk.Endpoints;

internal class GenreEndpoint : BaseAuthorizedEndpoint, IGenreEndpoint
{
    public GenreEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore, IIdentityEndpoint identityEndpoint) : base(httpClientFactory, tokenStore, identityEndpoint)
    {
    }
    
    public Task<ServiceResult<IList<GenreResult>>> GetAsync()
    {
        var result = GetAsync<ServiceResult<IList<GenreResult>>>("/api/genre");
        return result.ReadAsync();
    }
    
    public Task<ServiceResult<GenreResult>> CreateAsync(string name)
    {
        var result = PostAsJsonAsync<string, ServiceResult<GenreResult>>("/api/genre", name, true);
        return result.ReadAsync();
    }
}