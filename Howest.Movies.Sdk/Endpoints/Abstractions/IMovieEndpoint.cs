using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public interface IMovieEndpoint
{
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetAsync();
    Task<ServiceResult<MovieDetailResult>> GetAsync(Guid id);
    Task<Stream> GetPosterAsync(Guid id);
    Task<Stream> GetPosterThumbnailAsync(Guid id);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> GetTopAsync();
}