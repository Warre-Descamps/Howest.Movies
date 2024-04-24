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

        var admin = new User
        {
            Email = "admin@example.com",
            UserName = "admin",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(admin);
        await userManager.AddPasswordAsync(admin, "Test123!");

        var genres = new List<Genre>
        {
            new() { Name = "Animation" },
            new() { Name = "Family" },
            new() { Name = "Action" },
            new() { Name = "Adventure" },
            new() { Name = "Drama" },
            new() { Name = "Fantasy" },
            new() { Name = "Musical" },
            new() { Name = "Children" },
            new() { Name = "Crime" },
            new() { Name = "Comedy" },
            new() { Name = "Romance" },
            new() { Name = "Thriller" },
            new() { Name = "Horror" },
            new() { Name = "Mystery" },
            new() { Name = "Science Fiction" }
        };
        dbContext.Genres.AddRange(genres);
        await dbContext.SaveChangesAsync();

        var seedData = await File.ReadAllTextAsync(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "seed-data.json"));
        var movies = JsonConvert.DeserializeObject<JArray>(seedData);
        if (movies is null)
            throw new Exception("Could not read seed-data.json");

        dbContext.Movies.AddRange
        (
            from movie in movies
            select new Movie
            {
                AddedById = admin.Id,
                Title = movie["Title"]?.ToString(),
                Description = movie["Description"]?.ToString(),
                Director = movie["Director"]?.ToString(),
                ReleaseDate = DateTime.Parse(movie["ReleaseDate"]?.ToString()),
                Genres = genres
                    .Where(g => movie["Genres"].ToObject<List<string>>().Any(gs => gs == g.Name))
                    .Select(g => new MovieGenre { Genre = g })
                    .ToList(),
            }
        );
        await dbContext.SaveChangesAsync();
    }
}