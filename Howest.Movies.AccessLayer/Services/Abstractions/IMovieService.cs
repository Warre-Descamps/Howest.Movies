﻿using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;

namespace Howest.Movies.AccessLayer.Services.Abstractions;

public interface IMovieService
{
    Task<ServiceResult<MovieDetailResult>> FindByIdAsync(Guid id);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> FindAsync(MoviesFilter filter, PaginationFilter pagination);
    Task<ServiceResult<PaginationResult<IList<MovieResult>>>> FindTopAsync(PaginationFilter pagination);
    Task<ServiceResult<MovieDetailResult>> CreateAsync(MovieRequest request, Guid userId);
    Task<bool> ExistsAsync(Guid id);
    Task<ServiceResult<PaginationResult<IList<ReviewResult>>>> GetReviewsAsync(Guid id, PaginationFilter pagination);
    Task<ServiceResult<ReviewResult>> AddReviewAsync(Guid id, ReviewRequest request, Guid userId);
}