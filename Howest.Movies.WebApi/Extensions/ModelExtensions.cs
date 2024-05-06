using Grpc.Core;
using Howest.Movies.Dtos.Results;
using Howest.Movies.WebApi.Services;

namespace Howest.Movies.WebApi.Extensions;

public static class ModelExtensions
{
    public static void UpdatePosterInfo(this MovieResult movie, HttpRequest request)
    {
        var timestamp = PosterManagementService.GetTimeStamp(movie.Id);
        movie.Poster = $"{request.Scheme}://{request.Host}/api/movie{movie.Poster}{(string.IsNullOrWhiteSpace(timestamp) ? string.Empty : $"?t={timestamp}")}";
    }
    
    public static void UpdatePosterInfo(this IEnumerable<MovieResult> movies, HttpRequest request)
    {
        foreach (var movie in movies)
        {
            movie.UpdatePosterInfo(request);
        }
    }
    public static void UpdatePosterInfo(this IEnumerable<MovieResult> movies, ServerCallContext context)
    {
        foreach (var movie in movies)
        {
            movie.UpdatePosterInfo(context.GetHttpContext().Request);
        }
    }
}