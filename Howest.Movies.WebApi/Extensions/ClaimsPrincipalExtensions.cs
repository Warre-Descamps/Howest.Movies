using System.Security.Claims;

namespace Howest.Movies.WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal claimsPrincipal, out Guid userId)
    {
        userId = Guid.Empty;
        return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) is not null &&
               Guid.TryParse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value, out userId);
    }
}