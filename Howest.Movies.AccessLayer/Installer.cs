using Howest.Movies.AccessLayer.Repositories;
using Howest.Movies.AccessLayer.Repositories.Abstractions;
using Howest.Movies.AccessLayer.Services;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Howest.Movies.AccessLayer;

public static class Installer
{
    public static IServiceCollection InstallServices(this IServiceCollection services)
    {
        return services
            .AddDbContext<MovieDbContext>()
            .AddScoped<IGenreRepository, GenreRepository>()
            .AddScoped<IMovieRepository, MovieRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IGenreService, GenreService>()
            .AddScoped<IMovieService, MovieService>()
            .AddScoped<IReviewService, ReviewService>();
    }
}