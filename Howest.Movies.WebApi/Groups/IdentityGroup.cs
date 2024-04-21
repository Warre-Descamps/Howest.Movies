using Howest.Movies.Models;

namespace Howest.Movies.WebApi.Groups;

public static class IdentityGroup
{
    public static RouteGroupBuilder AddIdentity(this RouteGroupBuilder endpoints)
    {
        endpoints.MapGroup("/identity")
            .MapIdentityApi<User>();
        return endpoints;
    }
}