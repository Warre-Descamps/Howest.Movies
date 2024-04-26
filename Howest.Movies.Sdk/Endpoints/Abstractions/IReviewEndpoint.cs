using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public interface IReviewEndpoint
{
    Task<ServiceResult<ReviewResult>> UpdateAsync(Guid id, ReviewRequest reviewRequest, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}