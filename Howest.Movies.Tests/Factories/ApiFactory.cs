using Howest.Movies.Data;
using Howest.Movies.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Howest.Movies.Tests.Factories;

public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptors = services
                .Where(d => d.ServiceType == typeof(MovieDbContext) ||
                            d.ServiceType == typeof(DbContextOptions<MovieDbContext>))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }
    
            services.AddDbContext<MovieDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<MovieDbContext>();
            var userManager = scopedServices.GetRequiredService<UserManager<User>>();
            
            db.Database.EnsureCreated();

            try
            {
                SeedData(db, userManager).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred seeding the database with test messages. Error: {ex.Message}");
            }
        });
    }
    
    private static async Task SeedData(DbContext context, UserManager<User> userManager)
    {
        var user = new User { UserName = "TestUser", Email = "testuser@example.com" };
        await userManager.CreateAsync(user, "Test@123");

        await context.SaveChangesAsync();
    }
}