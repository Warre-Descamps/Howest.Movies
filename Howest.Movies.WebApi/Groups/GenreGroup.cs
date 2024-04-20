using Howest.Movies.Dtos.Core;
using Howest.Movies.Services.Services.Abstractions;

namespace Howest.Movies.WebApi.Groups;

public static class GenreGroup
{
    public static RouteGroupBuilder AddGenres(this RouteGroupBuilder endpoints)
    {
        endpoints.MapGet("/genre", async (IGenreService genreService) =>
        {
            var genres = await genreService.FindAsync();

            return Results.Ok(genres);
        });
        
        endpoints.MapPost("/genre", async (string name, IGenreService genreService) =>
        {
            var result = await genreService.CreateAsync(name);

            if (result.Data is null || !result.IsSuccess)
                return Results.BadRequest((ServiceResult) result);
            
            return Results.Created($"/genre/{result.Data!.Id}", result );
        });
        
        return endpoints;
    }
}