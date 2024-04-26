using System.Net.Http.Json;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
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

    public async Task<ServiceResult<MovieDetailResult>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync($"/api/movie/{id}", cancellationToken);
        return await response.ReadAsync<ServiceResult<MovieDetailResult>>(cancellationToken);
    }
    
    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync("/api/movie", cancellationToken);
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>(cancellationToken);
    }

    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(MoviesFilter filter, PaginationFilter pagination, CancellationToken cancellationToken = default)
    {
        var query = new QueryBuilder()
            .AddFilter(filter)
            .AddPagination(pagination)
            .Build();
        
        var response = await HttpClient.GetAsync($"/api/movie{query}", cancellationToken);
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>(cancellationToken);
    }
    
    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetTopAsync(CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.GetAsync("/api/movie/top", cancellationToken);
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>(cancellationToken);
    }

    public async Task<ServiceResult<MovieDetailResult>> CreateAsync(MovieRequest request, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.PostAsJsonAsync("/api/movie", request, cancellationToken);
        return await response.ReadAsync<ServiceResult<MovieDetailResult>>(cancellationToken);
    }

    public async Task<ServiceResult> AddPosterAsync(Guid id, Stream stream, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.PostAsync($"/api/movie/{id}/poster", new StreamContent(stream), cancellationToken);
        return await response.ReadAsync<ServiceResult>(cancellationToken);
    }

    public async Task<ServiceResult<PaginationResult<IList<ReviewResult>>>> GetReviewsAsync(Guid id, PaginationFilter pagination, CancellationToken cancellationToken = default)
    {
        var query = new QueryBuilder()
            .AddPagination(pagination)
            .Build();
        
        var response = await HttpClient.GetAsync($"/api/movie/{id}/review{query}", cancellationToken);
        return await response.ReadAsync<ServiceResult<PaginationResult<IList<ReviewResult>>>>(cancellationToken);
    }

    public async Task<ServiceResult<ReviewResult>> AddReviewAsync(Guid id, ReviewRequest request, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.PostAsJsonAsync($"/api/movie/{id}/review", request, cancellationToken);
        return await response.ReadAsync<ServiceResult<ReviewResult>>(cancellationToken);
    }
}