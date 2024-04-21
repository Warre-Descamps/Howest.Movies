using Howest.Movies.Models;
using Howest.Movies.WebApi.Extensions;

namespace Howest.Movies.WebApi.Groups;

public static class IdentityGroup
{
    public static RouteGroupBuilder AddIdentity(this RouteGroupBuilder endpoints)
    {
        endpoints.MapGroup("/identity")
            .MapCustomIdentityApi<User>();
        return endpoints;
    }
}