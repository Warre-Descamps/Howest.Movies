using Howest.Movies.Sdk.Abstractions;
using Howest.Movies.Sdk.Endpoints;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Howest.Movies.Sdk;

public static class Installer
{
    public static IServiceCollection InstallMoviesSdk(this IServiceCollection services)
    {
        services
            .AddScoped<IMoviesSdk, MoviesSdk>()
            .AddScoped<IMovieEndpoint, MovieEndpoint>()
            .AddScoped<IReviewEndpoint, ReviewEndpoint>()
            .AddScoped<IIdentityEndpoint, IdentityEndpoint>()
            .AddScoped<IGenreEndpoint, GenreEndpoint>()
            .AddHttpClient();

        return services;
    }
}