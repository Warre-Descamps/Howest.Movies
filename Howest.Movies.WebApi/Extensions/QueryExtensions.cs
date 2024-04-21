using Howest.Movies.Dtos.Filters;

namespace Howest.Movies.WebApi.Extensions;

public static class QueryExtensions
{
    public static MoviesFilter GetMoviesFilter(this IQueryCollection query)
    {
        return new MoviesFilter
        {
            Query = query["query"],
            Genres = query["genres"].OfType<string>().ToArray()
        };
    }
    
    public static PaginationFilter GetPaginationFilter(this IQueryCollection query)
    {
        var pagination = new PaginationFilter();
        if (query.ContainsKey("from") && int.TryParse(query["from"], out var from))
            pagination.From = from;
        if (query.ContainsKey("size") && int.TryParse(query["size"], out var size))
            pagination.Size = size;
        return pagination;
    }
}