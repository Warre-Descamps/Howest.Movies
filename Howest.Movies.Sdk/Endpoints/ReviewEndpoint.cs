using System.Net.Http.Json;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;

namespace Howest.Movies.Sdk.Endpoints;

internal class ReviewEndpoint : BaseEndpoint, IReviewEndpoint
{
    public ReviewEndpoint(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
    
    public async Task<ServiceResult<ReviewResult>> UpdateAsync(Guid id, ReviewRequest reviewRequest, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.PutAsJsonAsync($"/api/review/{id}", reviewRequest, cancellationToken);
        return await response.ReadAsync<ServiceResult<ReviewResult>>(cancellationToken);
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.DeleteAsync($"/api/review/{id}", cancellationToken);
        return await response.ReadAsync<ServiceResult>(cancellationToken);
    }
}