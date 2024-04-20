using System.Globalization;
using System.Security.Claims;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Services.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Howest.Movies.WebApi.Groups;

public static class MovieGroup
{
    public static RouteGroupBuilder AddMovies(this RouteGroupBuilder endpoints)
    {
        const string fileFormat = "yyyy-MM-ddTHHmmss.fffffff";
        var postersPath = Path.Combine(Directory.GetCurrentDirectory(), "posters");
        
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

        endpoints.MapGet("/moies/{id:guid}/poster", async (Guid id, IMovieService movieService) =>
        {
            var path = Path.Combine(postersPath, id.ToString());
            if (!Directory.Exists(path) || !await movieService.ExistsAsync(id))
                return Results.NotFound(new ServiceResult().NotFound());

            var fileInfo = Directory.GetFiles(path)
                .Select(f => new FileInfo(f))
                .Select(f => new
                {
                    fullPath = f.FullName,
                    fileName = f.Name[..^f.Extension.Length],
                    fileExtension = f.Extension
                })
                .MaxBy(s => DateTime.ParseExact(s.fileName, fileFormat, CultureInfo.InvariantCulture));
            if (fileInfo is null)
                return Results.NotFound(new ServiceResult().NotFound());
            
            var file = File.OpenRead(fileInfo.fullPath);
            return Results.File(file, $"image/{fileInfo.fileExtension[1..]}", id + fileInfo.fileExtension);
        });

        endpoints.MapGet("/movies/top", async ([FromQuery] PaginationFilter? paginationFilter, HttpRequest request, IMovieService movieService) =>
        {
            var pagination = new PaginationFilter();
            if (request.Query.ContainsKey("from") && int.TryParse(request.Query["from"], out var from))
                pagination.From = from;
            if (request.Query.ContainsKey("size") && int.TryParse(request.Query["size"], out var size))
                pagination.Size = size;
            
            var result = await movieService.FindTopAsync(pagination);
            return result.IsSuccess
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });

        endpoints.MapPost("/movies", async ([FromBody] MovieRequest request, ClaimsPrincipal user, IMovieService movieService) =>
        {
            if (user.FindFirst(ClaimTypes.NameIdentifier) is null ||
                !Guid.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value, out var userId))
                return Results.Unauthorized();

            var result = await movieService.CreateAsync(request, userId);
            return result.IsSuccess
                ? Results.Ok(result)
                : Results.BadRequest(result);
        })
        .RequireAuthorization();

        endpoints.MapPost("/movies/{id:guid}/poster", async (Guid id, IFormFile file, ClaimsPrincipal user, IMovieService movieService) =>
        {
            if (user.FindFirst(ClaimTypes.NameIdentifier) is null || !Guid.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value, out _))
                return Results.Unauthorized();
            
            if (!await movieService.ExistsAsync(id))
                return Results.NotFound(new ServiceResult().NotFound());
            
            if (file.Length == 0)
                return Results.BadRequest(new ServiceResult().BadRequest("File is empty!"));
            var fileInfo = new FileInfo(file.FileName);
            if (fileInfo.Extension != ".jpg")
                return Results.BadRequest(new ServiceResult().BadRequest("Invalid file extension. Only .jpg is allowed!"));
            
            var path = Path.Combine(postersPath, id.ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, DateTime.UtcNow.ToString(fileFormat) + fileInfo.Extension);
            await using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return Results.Ok();
        })
        .RequireAuthorization()
        .DisableAntiforgery();

        endpoints.MapPost("/movies/{id:guid}/review", async (Guid id, ClaimsPrincipal user) =>
        {

        })
        .RequireAuthorization();

        return endpoints;
    }
}