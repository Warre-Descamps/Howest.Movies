using Howest.Movies.Sdk.Abstractions;
using Howest.Movies.Sdk.Endpoints.Abstractions;

namespace Howest.Movies.Sdk;

public class MoviesSdk : IMoviesSdk
{
    public const string HttpClientName = "MoviesSdkHttpClient";

    public IMovieEndpoint Movies { get; }
    public IGenreEndpoint Genres { get; }
    public IReviewEndpoint Reviews { get; }
    public IIdentityEndpoint Identity { get; }
    
    public MoviesSdk(IMovieEndpoint movieEndpoint, IReviewEndpoint reviewEndpoint, IIdentityEndpoint identityEndpoint, IGenreEndpoint genres)
    {
        Movies = movieEndpoint;
        Reviews = reviewEndpoint;
        Identity = identityEndpoint;
        Genres = genres;
    }
}