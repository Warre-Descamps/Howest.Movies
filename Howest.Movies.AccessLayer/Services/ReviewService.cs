using AutoMapper;
using Howest.Movies.AccessLayer.Repositories.Abstractions;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Models;

namespace Howest.Movies.AccessLayer.Services;

public class ReviewService : IReviewService
{
    private readonly IMapper _mapper;
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IMapper mapper, IReviewRepository reviewRepository)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
    }
    
    public async Task<ServiceResult<ReviewResult>> UpdateAsync(Guid id, Guid userId, ReviewRequest request)
    {
        var existingReview = await _reviewRepository.GetByIdAsync(id);
        if (existingReview is null)
            return new ServiceResult<ReviewResult>().NotFound();
        
        if (existingReview.ReviewerId != userId)
            return new ServiceResult<ReviewResult>().Forbidden();
        
        var review = await _reviewRepository.UpdateAsync(id, new Review
        {
            Rating = request.Rating,
            Comment = request.Comment
        });

        return _mapper.Map<ReviewResult>(review);
    }

    public async Task<ServiceResult> DeleteAsync(Guid id, Guid userId)
    {
        var existingReview = await _reviewRepository.GetByIdAsync(id);
        if (existingReview is null)
            return new ServiceResult().NotFound();

        if (existingReview.ReviewerId != userId)
            return new ServiceResult().Forbidden();

        await _reviewRepository.DeleteAsync(id);
        return new ServiceResult();
    }
}