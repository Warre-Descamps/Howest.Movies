using Howest.Movies.Models;
using Howest.Movies.Services.Abstractions;

namespace Howest.Movies.Services.Repositories.Abstractions;

public interface IReviewRepository : IBaseRepository<Review, Guid>
{
    Task<Review?> GetByUserAsync(Guid movieId, Guid userId);
}