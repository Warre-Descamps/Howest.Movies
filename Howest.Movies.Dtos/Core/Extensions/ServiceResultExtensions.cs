﻿namespace Howest.Movies.Dtos.Core.Extensions;

public static class ServiceResultExtensions
{
    public static T AlreadyExists<T>(this T serviceResult) where T : ServiceResult
    {
        serviceResult.Messages.Add(new ServiceMessage(nameof(AlreadyExists), "The requested resource already exists.", MessageType.Warning));
        return serviceResult;
    }
    
    public static T NotFound<T>(this T serviceResult) where T : ServiceResult
    {
        serviceResult.Messages.Add(new ServiceMessage(nameof(NotFound), "The requested resource was not found.", MessageType.Error));
        return serviceResult;
    }
    
    public static T BadRequest<T>(this T serviceResult, string message = "") where T : ServiceResult
    {
        serviceResult.Messages.Add(new ServiceMessage(nameof(BadRequest), string.IsNullOrWhiteSpace(message) ? "The request was invalid." : message, MessageType.Error));
        return serviceResult;
    }
    
    public static T Forbidden<T>(this T serviceResult) where T : ServiceResult
    {
        serviceResult.Messages.Add(new ServiceMessage(nameof(Forbidden), "You are not allowed to perform this action.", MessageType.Error));
        return serviceResult;
    }
    
    public static T Unauthorized<T>(this T serviceResult) where T : ServiceResult
    {
        serviceResult.Messages.Add(new ServiceMessage(nameof(Unauthorized), "You are not authorized to perform this action.", MessageType.Error));
        return serviceResult;
    }
}