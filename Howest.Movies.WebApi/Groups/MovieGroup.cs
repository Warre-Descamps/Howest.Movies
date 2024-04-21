using System.Security.Claims;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Dtos.Core.Abstractions;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.WebApi.Extensions;
using Howest.Movies.WebApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Howest.Movies.WebApi.Groups;

public static class MovieGroup
{
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

        group.MapGet("/{id:guid}/poster-thumbnail", async ([FromRoute] Guid id, IPosterManagementService posterManagementService)
            => await posterManagementService.GetPosterThumbnail(id));

        group.MapGet("/{id:guid}/poster", async ([FromRoute] Guid id, IPosterManagementService posterManagementService)
            => await posterManagementService.GetPoster(id));

        // ReSharper disable RedundantAssignment
        group.MapGet("/top", async ([FromQuery] PaginationFilter? pagination, HttpRequest request, IMovieService movieService) =>
        // ReSharper restore RedundantAssignment
        {
            pagination = request.Query.GetPaginationFilter();
            
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

        group.MapPost("/{id:guid}/poster", async ([FromRoute] Guid id, IFormFile file, ClaimsPrincipal user, IPosterManagementService posterManagementService) =>
        {
            if (!user.TryGetUserId(out _))
                return Results.Unauthorized();

            return await posterManagementService.SavePoster(id, file);
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