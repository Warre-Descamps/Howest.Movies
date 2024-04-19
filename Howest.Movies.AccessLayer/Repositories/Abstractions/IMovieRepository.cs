using Howest.Movies.Models;
using Howest.Movies.Services.Abstractions;

namespace Howest.Movies.Services.Repositories.Abstractions;

public interface IMovieRepository : IBaseRepository<Movie, Guid>
{
    IQueryable<Movie> Find(string? query, string[] genres, int from, int size);
}