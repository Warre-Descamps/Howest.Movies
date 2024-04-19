using Howest.Movies.Models;
using Howest.Movies.Services.Abstractions;

namespace Howest.Movies.Services.Repositories.Abstractions;

public interface IReviewRepository : IBaseRepository<MovieReview, Guid>
{
}