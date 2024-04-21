using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Dtos.Core.Abstractions;

namespace Howest.Movies.WebApi.Groups;

public static class GenreGroup
{
    public static RouteGroupBuilder AddGenres(this RouteGroupBuilder endpoints, IReturnResolver resolver)
    {
        var group = endpoints.MapGroup("/genre");
        
        group.MapGet("", async (IGenreService genreService) =>
        {
            var result = await genreService.FindAsync();

            return result.GetReturn(resolver);
        });
        
        group.MapPost("", async (string name, IGenreService genreService) =>
        {
            var result = await genreService.CreateAsync(name);
            
            return result.GetReturn(resolver);
        })
        .RequireAuthorization();
        
        return endpoints;
    }
}