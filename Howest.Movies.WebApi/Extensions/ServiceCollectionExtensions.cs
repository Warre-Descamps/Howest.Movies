using Howest.Movies.Data;
using Howest.Movies.Dtos.Core.Abstractions;
using Howest.Movies.Models;
using Howest.Movies.WebApi.Implementations;
using Howest.Movies.WebApi.Services;
using Howest.Movies.WebApi.Services.Abstractions;
using Howest.Movies.WebApi.Services.Background;

namespace Howest.Movies.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InstallServices(this IServiceCollection services)
    {
        services
            .AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<MovieDbContext>();
        
        AccessLayer.Installer.InstallServices(services);
        services
            .AddScoped<IReturnResolver, ReturnResolver>()
            .AddScoped<IPosterManagementService, PosterManagementService>()
            .AddHostedService<MyBackgroundService>(provider =>
            {
                using var scope = provider.CreateScope();
                return new MyBackgroundService(scope.ServiceProvider.GetRequiredService<IPosterManagementService>());
            });
        
        return services;
    }
}