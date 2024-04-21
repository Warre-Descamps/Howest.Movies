using System.Net.Http.Json;
using System.Text.Json;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;

namespace Howest.Movies.Sdk.Endpoints;

public class GenreEndpoint : IGenreEndpoint
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GenreEndpoint(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<ServiceResult<IList<GenreResult>>> GetAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("/api/genre");
        return await response.ReadAsync<ServiceResult<IList<GenreResult>>>();
    }
    
    public async Task<ServiceResult<GenreResult>> PostAsync(string name)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/genre", name);
        return await response.ReadAsync<ServiceResult<GenreResult>>();
    }
}