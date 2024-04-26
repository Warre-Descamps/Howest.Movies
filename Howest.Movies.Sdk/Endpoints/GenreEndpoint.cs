using System.Net.Http.Json;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk.Endpoints;

internal class GenreEndpoint : BaseEndpoint, IGenreEndpoint
{
    public GenreEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore) : base(httpClientFactory, tokenStore)
    {
    }
    
    public async Task<ServiceResult<IList<GenreResult>>> GetAsync()
    {
        var response = await HttpClient.GetAsync("/api/genre");
        return await response.ReadAsync<ServiceResult<IList<GenreResult>>>();
    }
    
    public async Task<ServiceResult<GenreResult>> PostAsync(string name)
    {
        var client = await GetAuthorizedClientAsync();
        var response = await client.PostAsJsonAsync("/api/genre", name);
        return await response.ReadAsync<ServiceResult<GenreResult>>();
    }
}