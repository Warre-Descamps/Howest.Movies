namespace Howest.Movies.Dtos.Core.Extensions;

public static class ServiceResultExtensions
{
    public static ServiceResult<T> NotFound<T>(this ServiceResult<T> serviceResult)
    {
        serviceResult.Messages.Add(new ServiceMessage("NotFound", "The requested resource was not found.", MessageType.Warning));
        return serviceResult;
    }
}