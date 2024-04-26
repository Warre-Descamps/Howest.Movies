using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;
using Howest.Movies.Sdk.Helpers;

namespace Howest.Movies.Sdk.Endpoints;

internal class MovieEndpoint : BaseEndpoint, IMovieEndpoint
{
    public MovieEndpoint(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
    
    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync("/api/movie", cancellationToken);
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>(cancellationToken: cancellationToken);
    }

    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(MoviesFilter filter, PaginationFilter pagination, CancellationToken cancellationToken = default)
    {
        var query = new QueryBuilder()
            .AddFilter(filter)
            .AddPagination(pagination)
            .Build();
        
        var response = await HttpClient.GetAsync($"/api/movie{query}", cancellationToken);
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>(cancellationToken: cancellationToken);
    }

    public async Task<ServiceResult<MovieDetailResult>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync($"/api/movie/{id}", cancellationToken);
        return await response.ReadAsync<ServiceResult<MovieDetailResult>>(cancellationToken: cancellationToken);
    }
    
    public async Task<Stream> GetPosterAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync($"/api/movie/{id}/poster", cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
    
    public async Task<Stream> GetPosterThumbnailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync($"/api/movie/{id}/poster-thumbnail", cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
    
    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetTopAsync(CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync("/api/movie/top", cancellationToken);
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>(cancellationToken: cancellationToken);
    }
}