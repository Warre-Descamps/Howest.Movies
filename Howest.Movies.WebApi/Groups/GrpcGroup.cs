using Howest.Movies.WebApi.Services;

namespace Howest.Movies.WebApi.Groups;

public static class GrpcGroup
{
    public static RouteGroupBuilder AddGrpc(this RouteGroupBuilder endpoints)
    {
        endpoints.MapGroup("/grpc")
            .MapGrpcService<MovieGrpcService>();
        return endpoints;
    }
}