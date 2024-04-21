using Howest.Movies.AccessLayer.Abstractions;
using Howest.Movies.Models;

namespace Howest.Movies.AccessLayer.Repositories.Abstractions;

public interface IMovieRepository : IBaseRepository<Movie, Guid>
{
    Task<IList<Movie>> FindAsync(string? query, Guid[] genres, int from, int size);
    Task<Movie?> FindAsync(string title);
    Task<IList<Movie>> FindTopAsync(int from, int size);
    Task<bool> ExistsAsync(Guid id);
}