using Howest.Movies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Howest.Movies.Data;

public class MovieDbContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<MovieReview> Reviews { get; set; } = null!;
    
    public MovieDbContext() { }
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
            .Build();
        optionsBuilder.UseMySQL(configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string not found in appsettings.json."));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUserLogin<Guid>>(e =>
        {
            e.HasNoKey();
        });

        builder.Entity<IdentityUserToken<Guid>>(e =>
        {
            e.HasNoKey();
        });

        builder.Entity<IdentityUserRole<Guid>>(e =>
        {
            e.HasKey(ur => new { ur.UserId, ur.RoleId });
        });
        
        builder.Entity<MovieGenre>(e =>
        {
            e.HasKey(mg => new { mg.MovieId, mg.GenreId });
        });
        
        builder.Entity<Movie>(e =>
        {
            e.HasIndex(g => g.Title)
                .IsUnique();
        });
        
        builder.Entity<Genre>(e =>
        {
            e.HasIndex(g => g.Name)
                .IsUnique();
        });
    }
}