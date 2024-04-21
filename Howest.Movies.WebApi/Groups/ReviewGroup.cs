using System.Security.Claims;
using Howest.Movies.AccessLayer.Services.Abstractions;
using Howest.Movies.Dtos.Core.Abstractions;
using Howest.Movies.Dtos.Requests;
using Howest.Movies.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Howest.Movies.WebApi.Groups;

public static class ReviewGroup
{
    public static RouteGroupBuilder AddReviews(this RouteGroupBuilder endpoints, IReturnResolver resolver)
    {
        var group = endpoints.MapGroup("/review");
        
        group.MapPut("/{id:guid}", async ([FromRoute] Guid id, [FromBody] ReviewRequest reviewRequest, ClaimsPrincipal user, IReviewService reviewService) =>
        {
            if (!user.TryGetUserId(out var userId))
                return Results.Unauthorized();
            
            var result = await reviewService.UpdateAsync(id, userId, reviewRequest);

            return result.GetReturn(resolver);
        })
        .RequireAuthorization();

        group.MapDelete("/{id:guid}", async ([FromRoute] Guid id, ClaimsPrincipal user, IReviewService reviewService) =>
        {
            if (!user.TryGetUserId(out var userId))
                return Results.Unauthorized();

            var result = await reviewService.DeleteAsync(id, userId);

            return result.GetReturn(resolver);
        })
        .RequireAuthorization();

        return endpoints;
    }
}