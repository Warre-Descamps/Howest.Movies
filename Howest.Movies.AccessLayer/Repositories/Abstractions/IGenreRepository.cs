using System.Collections;
using Howest.Movies.AccessLayer.Abstractions;
using Howest.Movies.Models;

namespace Howest.Movies.AccessLayer.Repositories.Abstractions;

public interface IGenreRepository : IBaseRepository<Genre, Guid>
{
    Task<IList<Genre>> FindAsync(string[] genres);
    Task<Genre?> FindAsync(string name);
    Task<IList<Genre>> AddAsync(string[] names);
}