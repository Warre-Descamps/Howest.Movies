using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.AccessLayer.Services.Abstractions;

public interface IGenreService
{
    Task<ServiceResult<IList<string>>> FindAsync();
    
    Task<ServiceResult<GenreResult>> CreateAsync(string name);
}