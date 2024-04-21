using Howest.Movies.Data;
using Howest.Movies.Models;
using Howest.Movies.Services.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Howest.Movies.Services.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MovieDbContext _dbContext;

    public MovieRepository(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IList<Movie>> FindAsync()
    {
        return await _dbContext.Movies.ToListAsync();
    }

    public async Task<IList<Movie>> FindAsync(string? query, Guid[] genres, int from, int size)
    {
        var movies = _dbContext.Movies.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query))
            movies = movies.Where(m => m.Title.Contains(query) || m.Description.Contains(query));

        if (genres.Length > 0)
        {
            movies = movies.Where(m => m.Genres.Any(mg => genres.Any(gid => mg.GenreId == gid)));
        }
        
        var orderedMovies = !string.IsNullOrWhiteSpace(query)
            ? movies
                .OrderByDescending(m => m.Title.Contains(query))
                .ThenByDescending(m => m.Description.Contains(query))
                .ThenByDescending(m => m.Title)
                .ThenByDescending(m => m.Description)
            : movies
                .OrderByDescending(m => m.Title)
                .ThenByDescending(m => m.Description);
        
        var paginatedMovies = await orderedMovies
            .Skip(from)
            .Take(size)
            .ToListAsync();
        
        return paginatedMovies;
    }

    public Task<Movie?> FindAsync(string title)
    {
        return _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == title);
    }

    public async Task<IList<Movie>> FindTopAsync(int from, int size)
    {
        return await _dbContext.Movies
            .Where(m => m.Reviews.Count > 0)
            .OrderByDescending(m => m.Reviews.Sum(mr => mr.Rating) / m.Reviews.Count)
            .Skip(from)
            .Take(size)
            .ToListAsync();
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _dbContext.Movies.AnyAsync(m => m.Id == id);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        return _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);
    }
    
    public async Task<Movie> AddAsync(Movie movie)
    {
        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync();
        return movie;
    }
    
    public async Task<Movie?> UpdateAsync(Guid id, Movie movie)
    {
        var existingMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (existingMovie == null)
            return null;
        
        existingMovie.Title = movie.Title;
        existingMovie.Description = movie.Description;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        
        await _dbContext.SaveChangesAsync();
        return existingMovie;
    }
    
    public Task DeleteAsync(Guid id)
    {
        var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null) return Task.CompletedTask;
        
        _dbContext.Movies.Remove(movie);
        return _dbContext.SaveChangesAsync();
    }
}