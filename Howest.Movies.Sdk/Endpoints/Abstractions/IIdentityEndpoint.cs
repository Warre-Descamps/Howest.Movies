using Howest.Movies.Dtos.Core;
using Howest.Movies.Sdk.IdentityDtos;
using Howest.Movies.Sdk.IdentityDtos.Results;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public interface IIdentityEndpoint
{
    internal event Func<Task>? OnLogin;
    internal event Func<Task>? OnLogout;
    internal Task<ServiceResult<LoginResult>> RefreshAsync(string refreshToken);
    Task<ServiceResult> RegisterAsync(Request registerDto);
    Task<ServiceResult<LoginResult>> LoginAsync(Request request);
    Task<ServiceResult> LogoutAsync();
}