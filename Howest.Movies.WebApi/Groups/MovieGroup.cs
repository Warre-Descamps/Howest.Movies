using Howest.Movies.Dtos.Filters;
using Howest.Movies.Services.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Howest.Movies.WebApi.Groups;

public static class MovieGroup
{
    public static RouteGroupBuilder AddMovies(this RouteGroupBuilder endpoints)
    {
        endpoints.MapGet("/movies", async ([FromQuery] MoviesFilter? moviesFilter, [FromQuery] PaginationFilter? paginationFilter, HttpRequest request, IMovieService movieService) =>
        {
            var filter = new MoviesFilter
            {
                Query = request.Query["query"],
                Genres = request.Query["genres"].OfType<string>().ToArray(),
            };
            var pagination = new PaginationFilter();
            if (request.Query.ContainsKey("from") && int.TryParse(request.Query["from"], out var from))
                pagination.From = from;
            if (request.Query.ContainsKey("size") && int.TryParse(request.Query["size"], out var size))
                pagination.Size = size;
    
            var movies = await movieService.FindAsync(filter, pagination);
    
            return Results.Ok(movies);
        });

        endpoints.MapGet("/movies/{id:guid}", async (Guid id, IMovieService movieService) =>
        {
            var movie = await movieService.FindByIdAsync(id);
    
            return movie.IsSuccess
                ? Results.Ok(movie)
                : Results.BadRequest(movie);
        });

        endpoints.MapGet("/movies/{id:guid}/poster", async () =>
        {

        });

        endpoints.MapGet("/movies/top", async () =>
        {

        });

        endpoints.MapPost("/movies", async () =>
        {

        });

        endpoints.MapPost("/movies/{id:guid}/rate", async () =>
        {

        });

        return endpoints;
    }
}