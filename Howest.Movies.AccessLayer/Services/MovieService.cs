﻿using AutoMapper;
using Howest.Movies.AccessLayer.Extensions;
using Howest.Movies.AccessLayer.Repositories.Abstractions;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Howest.Movies.AccessLayer.Services;

public class MovieService : IMovieService
{
    private readonly IMapper _mapper;
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly UserManager<User> _userManager;

    public MovieService(IMapper mapper, IMovieRepository movieRepository, IGenreRepository genreRepository, IReviewRepository reviewRepository, UserManager<User> userManager)
    {
        _mapper = mapper;
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _reviewRepository = reviewRepository;
        _userManager = userManager;
    }
    
    public async Task<ServiceResult<MovieDetailResult>> FindByIdAsync(Guid id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null)
            return new ServiceResult<MovieDetailResult>().NotFound();
        
        return _mapper.Map<MovieDetailResult>(movie);
    }

    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> FindAsync(MoviesFilter filter, PaginationFilter pagination)
    {
        Guid[] genreIds = [];
        if (filter.Genres.Length > 0)
        {
            var genres = await _genreRepository.FindAsync(filter.Genres);
            genreIds = genres.Select(g => g.Id).ToArray();
        }
        var movies = await _movieRepository.FindAsync(filter.Query, genreIds, pagination.From, pagination.Size);

        return new PaginationResult<IList<MovieResult>>(_mapper.Map<List<MovieResult>>(movies), pagination.From, pagination.Size);
    }

    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> FindTopAsync(PaginationFilter pagination)
    {
        var movies = await _movieRepository.FindTopAsync(pagination.From, pagination.Size);
        
        return new PaginationResult<IList<MovieResult>>(_mapper.Map<List<MovieResult>>(movies), pagination.From, pagination.Size);
    }

    public async Task<ServiceResult<MovieDetailResult>> CreateAsync(MovieRequest request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return new ServiceResult<MovieDetailResult>().BadRequest();
        
        var existingMovie = await _movieRepository.FindAsync(request.Title);
        if (existingMovie != null)
            return new ServiceResult<MovieDetailResult>().AlreadyExists();
        
        Guid[] genreIds = [];
        if (request.Genres.Length > 0)
        {
            request.Genres = request.Genres
                .Select(s => s.RemoveSpecialCharacters())
                .ToArray();
            var genres = await _genreRepository.FindAsync(request.Genres);
            var missing = request.Genres.Except(genres.Select(g => g.Name)).ToArray();
            if (missing.Length > 0)
            {
                var missingGenres = await _genreRepository.AddAsync(missing);
                foreach (var genre in missingGenres)
                {
                    genres.Add(genre);
                }
            }
            genreIds = genres.Select(g => g.Id).ToArray();
        }

        var movie = await _movieRepository.AddAsync(new Movie
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseDate = request.ReleaseDate,
            Director = request.Director,
            AddedById = userId,
            Genres = genreIds.Select(g => new MovieGenre { GenreId = g }).ToList()
        });
        
        return _mapper.Map<MovieDetailResult>(movie);
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _movieRepository.ExistsAsync(id);
    }

    public async Task<ServiceResult<PaginationResult<IList<ReviewResult>>>> GetReviewsAsync(Guid id, PaginationFilter pagination)
    {
        var reviews = await _reviewRepository.FindAsync(id, pagination.From, pagination.Size);

        return new PaginationResult<IList<ReviewResult>>(_mapper.Map<List<ReviewResult>>(reviews), pagination.From, pagination.Size);
    }

    public async Task<ServiceResult<ReviewResult>> AddReviewAsync(Guid id, ReviewRequest request, Guid userId)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie is null)
            return new ServiceResult<ReviewResult>().NotFound();

        var existingReview = await _reviewRepository.GetByUserAsync(id, userId);
        if (existingReview is not null)
            return new ServiceResult<ReviewResult>().AlreadyExists();
        
        var review = await _reviewRepository.AddAsync(new Review
        {
            MovieId = id,
            Rating = request.Rating,
            Comment = request.Comment,
            ReviewerId = userId
        });
        var reviewer = await _userManager.FindByIdAsync(userId.ToString());
        review.Reviewer = reviewer;
        
        return _mapper.Map<ReviewResult>(review);
    }
}