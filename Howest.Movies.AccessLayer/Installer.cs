using Howest.Movies.Data;
using Howest.Movies.Services.Repositories;
using Howest.Movies.Services.Repositories.Abstractions;
using Howest.Movies.Services.Services;
using Howest.Movies.Services.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Howest.Movies.Services;

public static class Installer
{
    public static IServiceCollection InstallServices(this IServiceCollection services)
    {
        return services
            .AddDbContext<MovieDbContext>()
            .AddScoped<IGenreRepository, GenreRepository>()
            .AddScoped<IMovieRepository, MovieRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IMovieService, MovieService>();
    }
}