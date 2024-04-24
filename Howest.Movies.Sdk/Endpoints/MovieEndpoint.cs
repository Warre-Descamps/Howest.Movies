using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;
using Howest.Movies.Sdk.Helpers;

namespace Howest.Movies.Sdk.Endpoints;

public class MovieEndpoint : BaseEndpoint, IMovieEndpoint
{
    public MovieEndpoint(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
    
    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync()
    {
        var response = await HttpClient.GetAsync("/api/movie");
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>();
    }

    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(MoviesFilter filter, PaginationFilter pagination)
    {
        var query = new QueryBuilder()
            .AddFilter(filter)
            .AddPagination(pagination)
            .Build();
        
        var response = await HttpClient.GetAsync($"/api/movie{query}");
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>();
    }

    public async Task<ServiceResult<MovieDetailResult>> GetAsync(Guid id)
    {
        var response = await HttpClient.GetAsync($"/api/movie/{id}");
        return await response.ReadAsync<ServiceResult<MovieDetailResult>>();
    }
    
    public async Task<Stream> GetPosterAsync(Guid id)
    {
        var response = await HttpClient.GetAsync($"/api/movie/{id}/poster");
        return await response.Content.ReadAsStreamAsync();
    }
    
    public async Task<Stream> GetPosterThumbnailAsync(Guid id)
    {
        var response = await HttpClient.GetAsync($"/api/movie/{id}/poster-thumbnail");
        return await response.Content.ReadAsStreamAsync();
    }
    
    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetTopAsync()
    {
        var response = await HttpClient.GetAsync("/api/movie/top");
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>();
    }
}