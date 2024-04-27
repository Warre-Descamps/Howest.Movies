using Howest.Movies.Dtos.Core;
using Howest.Movies.Dtos.Core.Abstractions;
using Howest.Movies.Dtos.Core.Extensions;

namespace Howest.Movies.WebApi.Implementations;

public class ReturnResolver : IReturnResolver
{
    public object Resolve<T>(T serviceResult) where T : ServiceResult
    {
        if (serviceResult.Messages.All(m => m.Type != MessageType.Error))
        {
            return Results.Ok(serviceResult);
        }

        if (serviceResult.Messages.Count(m => m.Type == MessageType.Error) > 1)
        {
            return Results.BadRequest(serviceResult);
        }

        return serviceResult.Messages.First(m => m.Type == MessageType.Error)?.Code switch
        {
            nameof(ServiceResultExtensions.NotFound) => Results.NotFound(serviceResult),
            nameof(ServiceResultExtensions.Forbidden) => Results.Forbid(),
            nameof(ServiceResultExtensions.BadRequest) => Results.BadRequest(serviceResult),
            _ => Results.BadRequest(serviceResult)
        };
    }
}