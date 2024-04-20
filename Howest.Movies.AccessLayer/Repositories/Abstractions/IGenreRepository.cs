using Howest.Movies.Models;
using Howest.Movies.Services.Abstractions;

namespace Howest.Movies.Services.Repositories.Abstractions;

public interface IGenreRepository : IBaseRepository<Genre, Guid>
{
    Task<IList<Genre>> FindAsync(string[] genres);
    Task<Genre?> FindAsync(string name);
}