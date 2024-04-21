using Howest.Movies.Dtos.Results;

namespace Howest.Movies.WebApi.Extensions;

public static class ModelExtensions
{
    public static void UpdatePosterInfo(this IEnumerable<MovieResult> movies, HttpRequest request)
    {
        var baseUrl = $"{request.Scheme}://{request.Host}";
        foreach (var movie in movies)
        {
            movie.Poster = $"{baseUrl}/api/movie{movie.Poster}";
        }
    }
}