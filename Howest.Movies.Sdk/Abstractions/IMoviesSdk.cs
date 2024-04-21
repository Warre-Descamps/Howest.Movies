using Howest.Movies.Sdk.Endpoints.Abstractions;

namespace Howest.Movies.Sdk.Abstractions;

public interface IMoviesSdk
{
    IMovieEndpoint Movies { get; }
    IGenreEndpoint Genres { get; }
    IReviewEndpoint Reviews { get; }
    IIdentityEndpoint Identity { get; }
}