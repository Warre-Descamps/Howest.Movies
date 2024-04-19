using Howest.Movies.Services.Services.Abstractions;

namespace Howest.Movies.WebApi.Groups;

public static class GenreGroup
{
    public static RouteGroupBuilder AddGenres(this RouteGroupBuilder endpoints)
    {
        endpoints.MapGet("/genre", async () =>
        {
        });
        
        endpoints.MapPost("/genre", async () =>
        {

        });
        
        return endpoints;
    }
}