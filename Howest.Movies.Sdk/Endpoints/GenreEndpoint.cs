using System.Net.Http.Json;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;

namespace Howest.Movies.Sdk.Endpoints;

public class GenreEndpoint : BaseEndpoint, IGenreEndpoint
{
    public GenreEndpoint(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
    
    public async Task<ServiceResult<IList<GenreResult>>> GetAsync()
    {
        var response = await HttpClient.GetAsync("/api/genre");
        return await response.ReadAsync<ServiceResult<IList<GenreResult>>>();
    }
    
    public async Task<ServiceResult<GenreResult>> PostAsync(string name)
    {
        var response = await HttpClient.PostAsJsonAsync("/api/genre", name);
        return await response.ReadAsync<ServiceResult<GenreResult>>();
    }
}