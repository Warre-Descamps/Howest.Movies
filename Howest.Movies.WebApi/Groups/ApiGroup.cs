using Howest.Movies.Dtos.Core.Abstractions;

namespace Howest.Movies.WebApi.Groups;

public static class ApiGroup
{
    public static WebApplication AddApiGroup(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var returnResolver = scope.ServiceProvider.GetRequiredService<IReturnResolver>();
        
        app.MapGroup("/api")
            .AddGrpc()
            .AddMovies(returnResolver)
            .AddGenres(returnResolver)
            .AddReviews(returnResolver)
            .AddIdentity();

        return app;
    }
}