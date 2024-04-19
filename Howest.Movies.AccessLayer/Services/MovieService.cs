using AutoMapper;
using Howest.Movies.Common.Extensions;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Services.Repositories.Abstractions;
using Howest.Movies.Services.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Howest.Movies.Services.Services;

public class MovieService : IMovieService
{
    private readonly IMapper _mapper;
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMapper mapper, IMovieRepository movieRepository)
    {
        _mapper = mapper;
        _movieRepository = movieRepository;
    }

    public async Task<ServiceResult<PaginationResult<IList<MovieResult>>>> FindAsync(MoviesFilter filter, PaginationFilter pagination)
    {
        var movies = _movieRepository.Find(filter.Query, filter.Genres, pagination.From, pagination.Size);

        var orderedMovies = !string.IsNullOrWhiteSpace(filter.Query)
            ? movies
                .OrderByDescending(m => m.Title.Contains(filter.Query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(m => m.Title)
            : movies
                .OrderByDescending(m => m.Title);

        var paginatedMovies = _mapper.Map<List<MovieResult>>(await orderedMovies
            .ApplyPagination(pagination.From, pagination.Size)
            .ToListAsync());

        return new PaginationResult<IList<MovieResult>>(paginatedMovies, pagination.From, pagination.Size);
    }
}