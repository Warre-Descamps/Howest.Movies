using AutoMapper;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.Dtos.Results;
using Howest.Movies.Models;
using Howest.Movies.Services.Extensions;
using Howest.Movies.Services.Repositories.Abstractions;
using Howest.Movies.Services.Services.Abstractions;

namespace Howest.Movies.Services.Services;

public class GenreService : IGenreService
{
    private readonly IMapper _mapper;
    private readonly IGenreRepository _genreRepository;

    public GenreService(IMapper mapper, IGenreRepository genreRepository)
    {
        _mapper = mapper;
        _genreRepository = genreRepository;
    }
    
    public async Task<ServiceResult<IList<string>>> FindAsync()
    {
        var genres = await _genreRepository.FindAsync();

        return genres.Select(g => g.Name).ToList();
    }

    public async Task<ServiceResult<GenreResult>> CreateAsync(string name)
    {
        name = name.RemoveSpecialCharacters();
        if (string.IsNullOrWhiteSpace(name))
            return new ServiceResult<GenreResult>().BadRequest();
        
        var existingGenre = await _genreRepository.FindAsync(name);
        if (existingGenre != null)
            return new ServiceResult<GenreResult>(_mapper.Map<GenreResult>(existingGenre)).AlreadyExists();
        
        var genre = await _genreRepository.AddAsync(new Genre
        {
            Name = name,
        });

        return _mapper.Map<GenreResult>(genre);
    }
}