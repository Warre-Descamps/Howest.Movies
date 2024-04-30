using System.Reflection;
using Howest.Movies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Howest.Movies.Data;

public static class Initializer
{
    public static async Task SetupDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

        try
        {
            await dbContext.Database.EnsureCreatedAsync();
        }
        catch (InvalidOperationException e)
        {
            throw new Exception(e.Message);
        }
        catch (SqlException)
        {
            throw new Exception("Permission denied in database 'master'.");
        }
        catch
        {
            throw new Exception("Could not connect to the database server, please check your connection string in appsettings.json");
        }

        await serviceProvider.SeedDbAsync();
    }

    private static async Task SeedDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var userManager = services.GetRequiredService<UserManager<User>>();
        var dbContext = services.GetRequiredService<MovieDbContext>();

        if (dbContext.Movies.Any() || userManager.Users.Any())
            return;
        
        var seedData = await File.ReadAllTextAsync(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "seed-data.json"));
        var movies = JsonConvert.DeserializeObject<JArray>(seedData);
        if (movies is null)
            throw new Exception("Could not read seed-data.json");
        
        var existingGenres = dbContext.Genres.Select(g => g.Name).ToList();
        
        var genres = movies
            .SelectMany(t => t["Genres"]?.ToObject<List<string>>() ?? [])
            .Distinct()
            .Select(g => new Genre { Name = g })
            .ExceptBy(existingGenres, g => g.Name)
            .ToList();
        dbContext.Genres.AddRange(genres);
        await dbContext.SaveChangesAsync();
        
        var existingMovies = dbContext.Movies.Select(m => m.Title).ToList();
        
        var exeption = new Exception("Property missing.");
        dbContext.Movies.AddRange
        (
            movies
            .Select(json => new Movie
            {
                Title = json["Title"]?.ToString() ?? throw exeption,
                Description = json["Description"]?.ToString() ?? throw exeption,
                Director = json["Director"]?.ToString() ?? throw exeption,
                ReleaseDate = DateTime.Parse(json["ReleaseDate"]?.ToString() ?? throw exeption),
                Genres = genres.Where(g => (json["Genres"] ?? throw exeption).ToObject<List<string>>()!.Any(gs => gs == g.Name))
                    .Select(g => new MovieGenre { Genre = g })
                    .ToList(),
            })
            .ExceptBy(existingMovies, m => m.Title)
            .DistinctBy(m => m.Title)
        );
        await dbContext.SaveChangesAsync();
    }
}