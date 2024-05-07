using System.Security.Claims;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Abstractions;
using Howest.Movies.Dtos.Filters;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.Dtos.Results;
using Howest.Movies.WebApi.Extensions;
using Howest.Movies.WebApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Howest.Movies.WebApi.Groups;

public static class MovieGroup
{
    public static RouteGroupBuilder AddMovies(this RouteGroupBuilder endpoints, IReturnResolver resolver)
    {
        var group = endpoints.MapGroup("/movie");

        group.MapGet("/{id:guid}", async ([FromRoute] Guid id, HttpRequest request, IMovieService movieService) =>
        {
            var result = await movieService.FindByIdAsync(id);
            if (result.IsSuccess)
            {
                result.Data!.UpdatePosterInfo(request);
            }

            return result.GetReturn(resolver);
        }).Produces<ServiceResult<MovieDetailResult>>()
        .Produces<ServiceResult>(404);
        
        // ReSharper disable RedundantAssignment
        group.MapGet("", async ([FromQuery] MoviesFilter? filter, [FromQuery] PaginationFilter? pagination, HttpRequest request, IMovieService movieService) =>
        // ReSharper restore RedundantAssignment
        {
            filter = request.Query.GetMoviesFilter();
            pagination = request.Query.GetPaginationFilter();
    
            var result = await movieService.FindAsync(filter, pagination);
            if (result.IsSuccess)
            {
                result.Data!.Items.UpdatePosterInfo(request);
            }

            return result.GetReturn(resolver);
        }).Produces<ServiceResult<PaginationResult<IEnumerable<MovieResult>>>>();
        
        // ReSharper disable RedundantAssignment
        group.MapGet("/top", async ([FromQuery] PaginationFilter? pagination, HttpRequest request, IMovieService movieService) =>
        // ReSharper restore RedundantAssignment
        {
            pagination = request.Query.GetPaginationFilter();
            
            var result = await movieService.FindTopAsync(pagination);
            if (result.IsSuccess)
            {
                result.Data!.Items.UpdatePosterInfo(request);
            }
            
            return result.GetReturn(resolver);
        }).Produces<ServiceResult<PaginationResult<IEnumerable<MovieResult>>>>();
        
        group.MapPost("", async ([FromBody] MovieRequest request, ClaimsPrincipal user, IMovieService movieService) =>
        {
            if (!user.TryGetUserId(out var userId))
                return Results.Unauthorized();

            var result = await movieService.CreateAsync(request, userId);

            return result.IsSuccess
                ? Results.Created($"/movies/{result.Data!.Id}", result)
                : result.GetReturn(resolver);
        }).RequireAuthorization()
        .Produces<ServiceResult<MovieDetailResult>>(201)
        .Produces<ServiceResult>(404)
        .Produces(401);

        group.MapPost("/{id:guid}/poster", async ([FromRoute] Guid id, IFormFile file, ClaimsPrincipal user, IPosterManagementService posterManagementService) =>
        {
            if (!user.TryGetUserId(out _))
                return Results.Unauthorized();

            return await posterManagementService.SavePoster(id, file);
        }).RequireAuthorization()
        .DisableAntiforgery()
        .Produces<ServiceResult>(201)
        .Produces<ServiceResult>(400)
        .Produces<ServiceResult>(404)
        .Produces(401);

        group.MapGet("/poster-thumbnail/{id:guid}", async ([FromRoute] Guid id, IPosterManagementService posterManagementService)
            => await posterManagementService.GetPosterThumbnail(id))
            .Produces(200, contentType: "image/jpg");

        group.MapGet("/poster/{id:guid}", async ([FromRoute] Guid id, IPosterManagementService posterManagementService)
            => await posterManagementService.GetPoster(id))
            .Produces(200, contentType: "image/jpg");
        
        // ReSharper disable RedundantAssignment
        group.MapGet("/{id:guid}/review", async ([FromRoute] Guid id, [FromQuery] PaginationFilter? pagination, HttpRequest request, IMovieService movieService) =>
        // ReSharper restore RedundantAssignment
        {
            pagination = request.Query.GetPaginationFilter();
            
            var result = await movieService.GetReviewsAsync(id, pagination);
            
            return result.GetReturn(resolver);
        }).Produces<ServiceResult<PaginationResult<IEnumerable<ReviewResult>>>>();

        group.MapPost("/{id:guid}/review", async ([FromRoute] Guid id, [FromBody] ReviewRequest request, ClaimsPrincipal user, IMovieService movieService) =>
        {
            if (!user.TryGetUserId(out var userId))
                return Results.Unauthorized();
            
            var result = await movieService.AddReviewAsync(id, request, userId);
            
            return result.GetReturn(resolver);
        }).RequireAuthorization()
        .Produces<ServiceResult<ReviewResult>>()
        .Produces<ServiceResult>(400)
        .Produces<ServiceResult>(404);

        return endpoints;
    }
}