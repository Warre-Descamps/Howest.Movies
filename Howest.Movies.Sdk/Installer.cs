using Howest.Movies.Sdk.Abstractions;
using Howest.Movies.Sdk.Endpoints;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Howest.Movies.Sdk;

public static class Installer
{
    public static IServiceCollection InstallMoviesSdk(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)));
        
        services
            .AddScoped<IMoviesSdk, MoviesSdk>()
            .AddScoped<IMovieEndpoint, MovieEndpoint>()
            .AddScoped<IReviewEndpoint, ReviewEndpoint>()
            .AddScoped<IIdentityEndpoint, IdentityEndpoint>()
            .AddScoped<IGenreEndpoint, GenreEndpoint>()
            .AddHttpClient(MoviesSdk.HttpClientName, (provider, client) =>
            {
                client.BaseAddress = new Uri(provider.GetRequiredService<IOptions<ApiSettings>>().Value.BaseUrl);
            });

        return services;
    }
}