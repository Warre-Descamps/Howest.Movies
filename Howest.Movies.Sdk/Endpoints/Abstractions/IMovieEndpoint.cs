using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public interface IMovieEndpoint
{
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync(MoviesFilter filter, PaginationFilter pagination, CancellationToken cancellationToken = default);
    Task<ServiceResult<MovieDetailResult>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetTopAsync(CancellationToken cancellationToken = default);
}