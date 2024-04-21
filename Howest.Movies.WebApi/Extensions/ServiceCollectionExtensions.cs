using Howest.Movies.Dtos.Core.Abstractions;
using Howest.Movies.WebApi.Implementations;

namespace Howest.Movies.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection InstallServices(this IServiceCollection services)
    {
        AccessLayer.Installer.InstallServices(services);
        services.AddScoped<IReturnResolver, ReturnResolver>();
        
        return services;
    }
}