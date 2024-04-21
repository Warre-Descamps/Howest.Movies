using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;

namespace Howest.Movies.Sdk.Endpoints;

public class MovieEndpoint : IMovieEndpoint
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MovieEndpoint(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("/api/movie");
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>();
    }

    public async Task<ServiceResult<MovieDetailResult>> GetAsync(Guid id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"/api/movie/{id}");
        return await response.ReadAsync<ServiceResult<MovieDetailResult>>();
    }
    
    public async Task<Stream> GetPosterAsync(Guid id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"/api/movie/{id}/poster");
        return await response.Content.ReadAsStreamAsync();
    }
    
    public async Task<Stream> GetPosterThumbnailAsync(Guid id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"/api/movie/{id}/poster-thumbnail");
        return await response.Content.ReadAsStreamAsync();
    }
    
    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetTopAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("/api/movie/top");
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>();
    }
}