using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Howest.Movies.Data;

public class MovieDbContext : IdentityDbContext
{
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
}