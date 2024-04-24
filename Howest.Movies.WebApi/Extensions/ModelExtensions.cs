using Howest.Movies.Dtos.Results;

namespace Howest.Movies.WebApi.Extensions;

public static class ModelExtensions
{
    public static void UpdatePosterInfo(this MovieResult movie, HttpRequest request)
    {
        movie.Poster = $"{request.Scheme}://{request.Host}/api/movie{movie.Poster}";
    }
    
    public static void UpdatePosterInfo(this IEnumerable<MovieResult> movies, HttpRequest request)
    {
        foreach (var movie in movies)
        {
            movie.UpdatePosterInfo(request);
        }
    }
}