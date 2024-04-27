using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Extensions;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk.Endpoints;

internal class ReviewEndpoint : BaseAuthorizedEndpoint, IReviewEndpoint
{
    public ReviewEndpoint(IHttpClientFactory httpClientFactory, ITokenStore tokenStore, IIdentityEndpoint identityEndpoint) : base(httpClientFactory, tokenStore, identityEndpoint)
    {
    }

    public Task<ServiceResult<ReviewResult>> UpdateAsync(Guid id, ReviewRequest reviewRequest, CancellationToken cancellationToken = default)
    {
        var result = PutAsJsonAsync<ReviewRequest, ServiceResult<ReviewResult>>($"/api/review/{id}", reviewRequest, true);
        return result.ReadAsync(cancellationToken);
    }

    public Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = DeleteAsync<ServiceResult>($"/api/review/{id}", true);
        return result.ReadAsync(cancellationToken);
    }
}