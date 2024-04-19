using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.Services.Services.Abstractions;

public interface IMovieService
{
    Task<ServiceResult<MovieDetailResult>> FindByIdAsync(Guid id);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> FindAsync(MoviesFilter filter, PaginationFilter pagination);
}