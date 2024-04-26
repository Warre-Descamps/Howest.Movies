using Howest.Movies.AccessLayer.Abstractions;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Models;

namespace Howest.Movies.AccessLayer.Repositories.Abstractions;

public interface IReviewRepository : IBaseRepository<Review, Guid>
{
    Task<Review?> GetByUserAsync(Guid movieId, Guid userId);
    Task<IList<Review>> FindAsync(Guid movieId, int paginationFrom, int paginationSize);
}