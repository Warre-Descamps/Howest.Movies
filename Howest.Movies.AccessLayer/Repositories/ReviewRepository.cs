using Howest.Movies.AccessLayer.Repositories.Abstractions;
using Howest.Movies.Data;
using Howest.Movies.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.Movies.AccessLayer.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly MovieDbContext _dbContext;

    public ReviewRepository(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Review?> GetByIdAsync(Guid id)
    {
        return _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
    }

    public Task<Review?> GetByUserAsync(Guid movieId, Guid userId)
    {
        return _dbContext.Reviews.FirstOrDefaultAsync(r => r.MovieId == movieId && r.ReviewerId == userId);
    }

    public async Task<IList<Review>> FindAsync()
    {
        return await _dbContext.Reviews.ToListAsync();
    }

    public async Task<IList<Review>> FindAsync(Guid movieId, int paginationFrom, int paginationSize)
    {
        return await _dbContext.Reviews
            .Where(r => r.MovieId == movieId)
            .Include(r => r.Reviewer)
            .OrderByDescending(r => r.ReviewDate)
            .Skip(paginationFrom)
            .Take(paginationSize)
            .ToListAsync();
    }

    public async Task<Review> AddAsync(Review review)
    {
        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();
        return review;
    }

    public async Task<Review?> UpdateAsync(Guid id, Review review)
    {
        var existingReview = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        if (existingReview == null)
            return null;
        
        existingReview.Rating = review.Rating;
        existingReview.Comment = review.Comment;
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