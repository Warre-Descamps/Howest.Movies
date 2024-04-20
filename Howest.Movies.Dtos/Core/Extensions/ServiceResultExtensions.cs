namespace Howest.Movies.Dtos.Core.Extensions;

public static class ServiceResultExtensions
{
    public static ServiceResult<T> NotFound<T>(this ServiceResult<T> serviceResult)
    {
        serviceResult.Messages.Add(new ServiceMessage("NotFound", "The requested resource was not found.", MessageType.Warning));
        return serviceResult;
    }
    
    public static ServiceResult<T> AlreadyExists<T>(this ServiceResult<T> serviceResult)
    {
        serviceResult.Messages.Add(new ServiceMessage("AlreadyExists", "The requested resource already exists.", MessageType.Warning));
        return serviceResult;
    }
    
    public static ServiceResult<T> BadRequest<T>(this ServiceResult<T> serviceResult)
    {
        serviceResult.Messages.Add(new ServiceMessage("BadRequest", "The request was invalid.", MessageType.Error));
        return serviceResult;
    }
}