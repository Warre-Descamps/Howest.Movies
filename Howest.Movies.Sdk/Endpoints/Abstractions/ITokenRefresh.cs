namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public interface ITokenRefresh<T>
{
    Task<T> RefreshAsync();
}