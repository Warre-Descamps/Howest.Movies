namespace Howest.Movies.Dtos.Filters;

public class MoviesFilter : IParsable<MoviesFilter>
{
    public string? Query { get; set; }
    public string[] Genres { get; set; } = [];
    
    public static MoviesFilter Parse(string s, IFormatProvider? provider)
    {
        var filter = new MoviesFilter
        {
            Query = string.Empty
        };
        var genres = new List<string>();
        var parts = s.Split('&');
        foreach (var part in parts)
        {
            var keyValuePair = part.Split('=', 2);
            var key = keyValuePair[0].ToLower();
            var value = keyValuePair[1];
            switch (key)
            {
                case "query":
                    filter.Query += value;
                    break;
                case "genres":
                    genres.AddRange(value.Split(','));
                    break;
            }
        }
        filter.Genres = genres.ToArray();
        return filter;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out MoviesFilter result)
    {
        result = null!;
        if (s == null) return false;
        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch
        {
            return false;
        }
    }
}