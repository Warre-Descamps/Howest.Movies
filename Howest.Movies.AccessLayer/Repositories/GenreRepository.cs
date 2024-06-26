﻿using Howest.Movies.AccessLayer.Extensions;
using Howest.Movies.AccessLayer.Repositories.Abstractions;
using Howest.Movies.Data;
using Howest.Movies.Models;
using Microsoft.EntityFrameworkCore;

namespace Howest.Movies.AccessLayer.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly MovieDbContext _dbContext;

    public GenreRepository(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Genre>> FindAsync()
    {
        return await _dbContext.Genres.ToListAsync();
    }

    public async Task<IList<Genre>> FindAsync(string[] genreNames)
    {
        genreNames = genreNames
            .Select(s => s.RemoveSpecialCharacters())
            .ToArray();
        
        var genres = await _dbContext.Genres
            .Where(g => genreNames.Contains(g.Name)).ToListAsync();

        return genres;
    }

    public async Task<Genre?> FindAsync(string name)
    {
        return await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task<IList<Genre>> AddAsync(string[] names)
    {
        var genres = names
            .Select(n => new Genre { Name = n.RemoveSpecialCharacters() })
            .ToList();
        
        _dbContext.Genres.AddRange(genres);
        await _dbContext.SaveChangesAsync();
        return genres;
    }

    public Task<Genre?> GetByIdAsync(Guid id)
    {
        return _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Genre> AddAsync(Genre genre)
    {
        _dbContext.Genres.Add(genre);
        await _dbContext.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre?> UpdateAsync(Guid id, Genre genre)
    {
        var existingGenre = await _dbContext.Genres.FirstOrDefaultAsync(m => m.Id == id);
        if (existingGenre == null)
            return null;
        
        existingGenre.Name = genre.Name;
        
        await _dbContext.SaveChangesAsync();
        return existingGenre;
    }

    public Task DeleteAsync(Guid id)
    {
        var genre = _dbContext.Genres.FirstOrDefault(g => g.Id == id);
        if (genre == null) return Task.CompletedTask;
        
        _dbContext.Genres.Remove(genre);
        return _dbContext.SaveChangesAsync();
    }
}