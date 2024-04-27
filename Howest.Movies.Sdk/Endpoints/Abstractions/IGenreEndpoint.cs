using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public interface IGenreEndpoint
{
    Task<ServiceResult<IList<GenreResult>>> GetAsync();
    Task<ServiceResult<GenreResult>> CreateAsync(string name);
}