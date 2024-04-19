using Howest.Movies.Dtos.Core;

namespace Howest.Movies.Dtos.Filters;

public class PaginationFilter : IParsable<PaginationFilter>
{
    public int From { get; set; } = 0;
    public int Size { get; set; } = 10;
    
    public static PaginationFilter Parse(string s, IFormatProvider? provider)
    {
        var filter = new PaginationFilter();
        var parts = s.Split('&');
        foreach (var part in parts)
        {
            var keyValuePair = part.Split('=', 2);
            var key = keyValuePair[0].ToLower();
            var value = keyValuePair[1];
            if (!int.TryParse(value, out var intValue))
                continue;
            switch (key)
            {
                case "from":
                    filter.From = intValue;
                    break;
                case "size":
                    filter.Size = intValue;
                    break;
            }
        }
        return filter;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out PaginationFilter result)
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