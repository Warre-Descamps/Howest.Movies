using Howest.Movies.Dtos.Core;
using Howest.Movies.Sdk.IdentityDtos;
using Howest.Movies.Sdk.IdentityDtos.Results;

namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public interface IIdentityEndpoint
{
    Task<ServiceResult> RegisterAsync(Request registerDto);
    Task<ServiceResult<LoginResult>> LoginAsync(Request request);
    Task<ServiceResult<LoginResult>> RefreshAsync(string refreshToken);
}