using System.Globalization;
using System.Security.Claims;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Abstractions;
using Howest.Movies.Dtos.Core.Extensions;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Howest.Movies.WebApi.Groups;

public static class MovieGroup
{
    private static readonly string PostersPath = Path.Combine(Directory.GetCurrentDirectory(), "posters");
    private const string FileFormat = "yyyy-MM-ddTHHmmss.fffffff";
    private const string Thumbnail = "-thumbnail";
    
    private static async Task<ServiceResult<(string fullPath, string fileName, string fileExtension)>> GetFileInfoAsync(Guid id, IMovieService movieService)
    {
        var path = Path.Combine(PostersPath, id.ToString());
        if (!Directory.Exists(path) || !await movieService.ExistsAsync(id))
            return new ServiceResult<(string, string, string)>().NotFound();

        var fileInfo = Directory.GetFiles(path)
            .Select(f => new FileInfo(f))
            .Select(f => (
                fullPath: f.FullName,
                fileName: f.Name[..^f.Extension.Length],
                fileExtension: f.Extension
            ))
            .Where(t => !t.fileName.EndsWith(Thumbnail))
            .MaxBy(s => DateTime.ParseExact(s.fileName, FileFormat, CultureInfo.InvariantCulture));
        
        return fileInfo.fileName is null
            ? new ServiceResult<(string, string, string)>().NotFound()
            : fileInfo;
    }
    
    public static RouteGroupBuilder AddMovies(this RouteGroupBuilder endpoints, IReturnResolver resolver)
    {
        var group = endpoints.MapGroup("/movie");
        
        // ReSharper disable RedundantAssignment
        group.MapGet("", async ([FromQuery] MoviesFilter? filter, [FromQuery] PaginationFilter? pagination, HttpRequest request, IMovieService movieService) =>
        // ReSharper restore RedundantAssignment
        {
            filter = request.Query.GetMoviesFilter();
            pagination = request.Query.GetPaginationFilter();
    
            var result = await movieService.FindAsync(filter, pagination);

            return result.GetReturn(resolver);
        });

        group.MapGet("/{id:guid}", async ([FromRoute] Guid id, IMovieService movieService) =>
        {
            var result = await movieService.FindByIdAsync(id);
    
            return result.GetReturn(resolver);
        });

        group.MapGet("/{id:guid}/poster-thumbnail", async ([FromRoute] Guid id, IMovieService movieService) =>
        {
            var fileInfo = await GetFileInfoAsync(id, movieService);
            if (!fileInfo.IsSuccess)
                return Results.File(Path.Combine(PostersPath, "default.jpg"), "image/jpg", "default.jpg");
            
            var file = File.OpenRead(fileInfo.Data.fullPath[..^fileInfo.Data.fileExtension.Length] + Thumbnail + fileInfo.Data.fileExtension);
            return Results.File(file, $"image/{fileInfo.Data.fileExtension[1..]}", id + Thumbnail + fileInfo.Data.fileExtension);
        });

        group.MapGet("/{id:guid}/poster", async ([FromRoute] Guid id, IMovieService movieService) =>
        {
            var fileInfo = await GetFileInfoAsync(id, movieService);
            if (!fileInfo.IsSuccess)
                return Results.File(Path.Combine(PostersPath, "default.jpg"), "image/jpg", "default.jpg");
            
            var file = File.OpenRead(fileInfo.Data.fullPath);
            return Results.File(file, $"image/{fileInfo.Data.fileExtension[1..]}", id + fileInfo.Data.fileExtension);
        });

        group.MapGet("/top", async ([FromQuery] PaginationFilter? paginationFilter, HttpRequest request, IMovieService movieService) =>
        {
            var pagination = new PaginationFilter();
            if (request.Query.ContainsKey("from") && int.TryParse(request.Query["from"], out var from))
                pagination.From = from;
            if (request.Query.ContainsKey("size") && int.TryParse(request.Query["size"], out var size))
                pagination.Size = size;
            
            var result = await movieService.FindTopAsync(pagination);
            
            return result.GetReturn(resolver);
        });

        group.MapPost("", async ([FromBody] MovieRequest request, ClaimsPrincipal user, IMovieService movieService) =>
        {
            if (!user.TryGetUserId(out var userId))
                return Results.Unauthorized();

            var result = await movieService.CreateAsync(request, userId);
            
            return result.IsSuccess
                ? Results.Created($"/movies/{result.Data!.Id}", result)
                : Results.BadRequest(result);
        })
        .RequireAuthorization();

        group.MapPost("/{id:guid}/poster", async ([FromRoute] Guid id, IFormFile file, ClaimsPrincipal user, IMovieService movieService) =>
        {
            if (!user.TryGetUserId(out _))
                return Results.Unauthorized();
            
            if (!await movieService.ExistsAsync(id))
                return Results.NotFound(new ServiceResult().NotFound());
            
            if (file.Length == 0)
                return Results.BadRequest(new ServiceResult().BadRequest("File is empty!"));
            var fileInfo = new FileInfo(file.FileName);
            if (fileInfo.Extension != ".jpg")
                return Results.BadRequest(new ServiceResult().BadRequest("Invalid file extension. Only .jpg is allowed!"));
            
            var path = Path.Combine(PostersPath, id.ToString());
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, DateTime.UtcNow.ToString(FileFormat) + fileInfo.Extension);
            await using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            
            using (var image = await Image.LoadAsync(path))
            {
                if (image.Size is { Width: <= 200, Height: <= 300 }) return Results.Ok();
                    
                var ratio = image.Size.Height / (image.Size.Width * 1f);
                image.Mutate(x => x.Resize(200, (int) (200 * ratio)));
                await image.SaveAsync(path[..^fileInfo.Extension.Length] + Thumbnail + fileInfo.Extension);
            }

            return Results.Created();
        })
        .RequireAuthorization()
        .DisableAntiforgery();

        group.MapPost("/{id:guid}/review", async ([FromRoute] Guid id, [FromBody] ReviewRequest request, ClaimsPrincipal user, IMovieService movieService) =>
        {
            if (!user.TryGetUserId(out var userId))
                return Results.Unauthorized();
            
            var result = await movieService.AddReviewAsync(id, request, userId);
            
            return result.GetReturn(resolver);
        })
        .RequireAuthorization();

        return endpoints;
    }
}