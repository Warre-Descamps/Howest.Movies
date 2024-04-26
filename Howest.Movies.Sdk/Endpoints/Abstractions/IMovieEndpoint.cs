using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public interface IMovieEndpoint
{
    Task<ServiceResult<MovieDetailResult>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(MoviesFilter filter, PaginationFilter pagination, CancellationToken cancellationToken = default);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetTopAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<MovieDetailResult>> CreateAsync(MovieRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult> AddPosterAsync(Guid id, Stream stream, CancellationToken cancellationToken = default);
    Task<ServiceResult<PaginationResult<IList<ReviewResult>>>> GetReviewsAsync(Guid id, PaginationFilter pagination, CancellationToken cancellationToken = default);
    Task<ServiceResult<ReviewResult>> AddReviewAsync(Guid id, ReviewRequest request, CancellationToken cancellationToken = default);
}