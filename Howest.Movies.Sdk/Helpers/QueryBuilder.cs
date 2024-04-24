using System.Collections;
using System.Text;

namespace Howest.Movies.Sdk.Helpers;

internal class QueryBuilder
{
    private readonly StringBuilder _query = new();

    public QueryBuilder AddFilter(object? filter)
    {
        if (filter is null)
        {
            return this;
        }

        var properties = filter.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(filter);
            if (value is null)
            {
                continue;
            }
            
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                foreach (var item in (IEnumerable)value)
                {
                    _query.Append(_query.Length == 0 ? "?" : "&");

                    _query.Append($"{property.Name}={item}");
                }
            }
            else
            {
                _query.Append(_query.Length == 0 ? "?" : "&");

                _query.Append($"{property.Name}={value}");
            }
        }

        return this;
    }

    public QueryBuilder AddPagination(object? pagination)
    {
        if (pagination is null)
        {
            return this;
        }

        var properties = pagination.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(pagination);
            if (value is null)
            {
                continue;
            }

            _query.Append(_query.Length == 0 ? "?" : "&");

            _query.Append($"{property.Name}={value}");
        }

        return this;
    }

    public string Build()
    {
        return _query.ToString();
    }
}