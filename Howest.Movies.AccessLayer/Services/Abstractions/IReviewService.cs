using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.AccessLayer.Services.Abstractions;

public interface IReviewService
{
    Task<ServiceResult<ReviewResult>> UpdateAsync(Guid id, Guid userId, ReviewRequest request);
    Task<ServiceResult> DeleteAsync(Guid id, Guid userId);
}