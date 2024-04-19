using Howest.Movies.Data;
using Howest.Movies.Models;
using Howest.Movies.Services.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Howest.Movies.Services.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly MovieDbContext _dbContext;

    public ReviewRepository(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<MovieReview>> FindAsync()
    {
        return await _dbContext.Reviews.ToListAsync();
    }

    public Task<MovieReview?> GetByIdAsync(Guid id)
    {
        return _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<MovieReview> AddAsync(MovieReview review)
    {
        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();
        return review;
    }

    public async Task<MovieReview?> UpdateAsync(Guid id, MovieReview review)
    {
        var existingReview = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        if (existingReview == null)
            return null;
        
        existingReview.Rating = review.Rating;
        existingReview.ReviewText = review.ReviewText;
        existingReview.ReviewDate = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        return existingReview;
    }

    public Task DeleteAsync(Guid id)
    {
        var review = _dbContext.Reviews.FirstOrDefault(r => r.Id == id);
        if (review == null) return Task.CompletedTask;
        
        _dbContext.Reviews.Remove(review);
        return _dbContext.SaveChangesAsync();
    }
}