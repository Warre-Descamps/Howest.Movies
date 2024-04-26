using Howest.Movies.Sdk.Abstractions;
using Howest.Movies.Sdk.Endpoints;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Settings;
using Howest.Movies.Sdk.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Howest.Movies.Sdk;

public static class Installer
{
    public static IServiceCollection InstallMoviesSdk<TTokenStore>(this IServiceCollection services, IConfiguration configuration) where TTokenStore : class, ITokenStore
    {
        services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)));
        
        services
            .AddTransient<ITokenStore, TTokenStore>()
            .AddScoped<IIdentityEndpoint, IdentityEndpoint>()
            .AddScoped<IMovieEndpoint, MovieEndpoint>()
            .AddScoped<IReviewEndpoint, ReviewEndpoint>()
            .AddScoped<IGenreEndpoint, GenreEndpoint>()
            .AddSingleton<IMoviesSdk>(provider =>
            {
                using var scope = provider.CreateScope();
                var tokenStore = scope.ServiceProvider.GetRequiredService<ITokenStore>();
                var identityEndpoint = scope.ServiceProvider.GetRequiredService<IIdentityEndpoint>();
                var movieEndpoint = scope.ServiceProvider.GetRequiredService<IMovieEndpoint>();
                var reviewEndpoint = scope.ServiceProvider.GetRequiredService<IReviewEndpoint>();
                var genreEndpoint = scope.ServiceProvider.GetRequiredService<IGenreEndpoint>();
                return new MoviesSdk(tokenStore, identityEndpoint, movieEndpoint, reviewEndpoint, genreEndpoint);
            })
            .AddHttpClient(MoviesSdk.HttpClientName, (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApiSettings>>();
                if (options.Value.BaseUrl is null)
                    throw new InvalidOperationException("Base URL is not set in configuration.");
                client.BaseAddress = new Uri(options.Value.BaseUrl);
            });

        return services;
    }
}