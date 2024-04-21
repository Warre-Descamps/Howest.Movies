namespace Howest.Movies.Dtos.Core.Abstractions;

public interface IReturnResolver
{
    object Resolve<T>(T serviceResult) where T : ServiceResult;
}