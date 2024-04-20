using Howest.Movies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Howest.Movies.Data;

public static class Seeder
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
            throw new Exception("Could not connect to the server, please check your connection string in appsettings.json");
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
            new() { Name = "Children" }
        };
        dbContext.Genres.AddRange(genres);
        await dbContext.SaveChangesAsync();

        dbContext.Movies.AddRange
        (
            new Movie
            {
                AddedByUser = admin,
                Title = "How to Train Your Dragon",
                Description = "As the son of a Viking leader on the cusp of manhood, shy Hiccup Horrendous Haddock III faces a rite of passage: he must kill a dragon to prove his warrior mettle. But after downing a feared dragon, he realizes that he no longer wants to destroy it, and instead befriends the beast – which he names Toothless – much to the chagrin of his warrior father.",
                ReleaseDate = new DateTime(2010, 3, 26),
                Director = "Chris Sanders",
                Genres = new List<MovieGenre>
                {
                    new() { GenreId = genres.Single(g => g.Name == "Fantasy").Id },
                    new() { GenreId = genres.Single(g => g.Name == "Animation").Id },
                    new() { GenreId = genres.Single(g => g.Name == "Family").Id },
                    new() { GenreId = genres.Single(g => g.Name == "Action").Id }
                }
            },
            new Movie
            {
                AddedByUser = admin,
                Title = "The Lion King",
                Description = "A young lion prince is cast out of his pride by his cruel uncle, who claims he killed his father. While the uncle rules with an iron paw, the prince grows up beyond the Savannah, living by a philosophy: No worries for the rest of your days. But when his past comes to haunt him, the young prince must decide his fate: will he remain an outcast or face his demons and become what he needs to be?",
                ReleaseDate = new DateTime(1994, 6, 24),
                Director = "Roger Allers",
                Genres = new List<MovieGenre>
                {
                    new() { GenreId = genres.Single(g => g.Name == "Family").Id },
                    new() { GenreId = genres.Single(g => g.Name == "Animation").Id },
                    new() { GenreId = genres.Single(g => g.Name == "Drama").Id },
                    new() { GenreId = genres.Single(g => g.Name == "Musical").Id },
                    new() { GenreId = genres.Single(g => g.Name == "Adventure").Id },
                    new() { GenreId = genres.Single(g => g.Name == "Children").Id }
                }
            }
        );
        await dbContext.SaveChangesAsync();
    }
}