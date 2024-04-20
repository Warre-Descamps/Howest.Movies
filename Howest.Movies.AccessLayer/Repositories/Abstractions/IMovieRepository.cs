using Howest.Movies.Models;
using Howest.Movies.Services.Abstractions;

namespace Howest.Movies.Services.Repositories.Abstractions;

public interface IMovieRepository : IBaseRepository<Movie, Guid>
{
    Task<IList<Movie>> FindAsync(string? query, Guid[] genres, int from, int size);
    Task<Movie?> FindAsync(string title);
    Task<IList<Movie>> FindTopAsync(int from, int size);
    Task<bool> ExistsAsync(Guid id);
}