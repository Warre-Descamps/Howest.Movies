using System.Net.Http.Json;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk.Endpoints;

internal class ReviewEndpoint : BaseEndpoint, IReviewEndpoint
{
    public ReviewEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore) : base(httpClientFactory, tokenStore)
    {
    }
    
    public async Task<ServiceResult<ReviewResult>> UpdateAsync(Guid id, ReviewRequest reviewRequest, CancellationToken cancellationToken = default)
    {
        var client = await GetAuthorizedClientAsync();
        var response = await client.PutAsJsonAsync($"/api/review/{id}", reviewRequest, cancellationToken);
        return await response.ReadAsync<ServiceResult<ReviewResult>>(cancellationToken);
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await GetAuthorizedClientAsync();
        var response = await client.DeleteAsync($"/api/review/{id}", cancellationToken);
        return await response.ReadAsync<ServiceResult>(cancellationToken);
    }
}