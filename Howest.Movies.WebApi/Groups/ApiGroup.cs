using Asp.Versioning.Conventions;
using Howest.Movies.Dtos.Core.Abstractions;

namespace Howest.Movies.WebApi.Groups;

public static class ApiGroup
{
    public static WebApplication AddApiGroup(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var returnResolver = scope.ServiceProvider.GetRequiredService<IReturnResolver>();

        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(1, 0)
            .Build();
        
        app.MapGroup("/api")
            .AddGrpc()
            .AddMovies(returnResolver)
            .AddGenres(returnResolver)
            .AddReviews(returnResolver)
            .AddIdentity()
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0);;

        return app;
    }
}