using System.Net.Http.Headers;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;
using Howest.Movies.Sdk.Helpers;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk.Endpoints;

internal class MovieEndpoint : BaseAuthorizedEndpoint, IMovieEndpoint
{
    public MovieEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore, IIdentityEndpoint identityEndpoint) : base(httpClientFactory, tokenStore, identityEndpoint)
    {
    }

    public Task<ServiceResult<MovieDetailResult>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = GetAsync<ServiceResult<MovieDetailResult>>($"/api/movie/{id}");
        return result.ReadAsync(cancellationToken);
    }
    
    public Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(CancellationToken cancellationToken = default)
    {
        var result = GetAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>("/api/movie");
        return result.ReadAsync(cancellationToken);
    }

    public Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(MoviesFilter filter, PaginationFilter pagination, CancellationToken cancellationToken = default)
    {
        var query = new QueryBuilder()
            .AddFilter(filter)
            .AddPagination(pagination)
            .Build();
        
        var result = GetAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>($"/api/movie{query}");
        return result.ReadAsync(cancellationToken);
    }
    
    public Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetTopAsync(CancellationToken cancellationToken = default)
    {
        var result = GetAsync<ServiceResult<PaginationResult<IList<MovieResult>>>>("/api/movie/top");
        return result.ReadAsync(cancellationToken);
    }

    public Task<ServiceResult<MovieDetailResult>> CreateAsync(MovieRequest request, CancellationToken cancellationToken = default)
    {
        var result = PostAsJsonAsync<MovieRequest, ServiceResult<MovieDetailResult>>("/api/movie", request, true);
        return result.ReadAsync(cancellationToken);
    }

    public Task<ServiceResult> AddPosterAsync(Guid id, string fileName, Stream stream, CancellationToken cancellationToken = default)
    {
        var content = new MultipartFormDataContent();
        
        content.Add(new StreamContent(stream)
        {
            Headers = { ContentType = MediaTypeHeaderValue.Parse("multipart/form-data") }
        }, "file", fileName);
        
        var result = PostAsync<ServiceResult>($"/api/movie/{id}/poster", content, true);
        return result.ReadAsync(cancellationToken);
    }

    public Task<ServiceResult<PaginationResult<IList<ReviewResult>>>> GetReviewsAsync(Guid id, PaginationFilter pagination, CancellationToken cancellationToken = default)
    {
        var query = new QueryBuilder()
            .AddPagination(pagination)
            .Build();

        var result = GetAsync<ServiceResult<PaginationResult<IList<ReviewResult>>>>($"/api/movie/{id}/review{query}");
        return result.ReadAsync(cancellationToken);
    }

    public Task<ServiceResult<ReviewResult>> AddReviewAsync(Guid id, ReviewRequest request, CancellationToken cancellationToken = default)
    {
        var result = PostAsJsonAsync<ReviewRequest, ServiceResult<ReviewResult>>($"/api/movie/{id}/review", request, true);
        return result.ReadAsync(cancellationToken);
    }
}