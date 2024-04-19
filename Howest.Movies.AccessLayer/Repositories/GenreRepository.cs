using Howest.Movies.Data;
using Howest.Movies.Models;
using Howest.Movies.Services.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Howest.Movies.Services.Repositories;

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