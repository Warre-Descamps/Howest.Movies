﻿using AutoMapper;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Services.Repositories.Abstractions;
using Howest.Movies.Services.Services.Abstractions;

namespace Howest.Movies.Services.Services;

public class MovieService : IMovieService
{
    private readonly IMapper _mapper;
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;

    public MovieService(IMapper mapper, IMovieRepository movieRepository, IGenreRepository genreRepository)
    {
        _mapper = mapper;
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
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
}