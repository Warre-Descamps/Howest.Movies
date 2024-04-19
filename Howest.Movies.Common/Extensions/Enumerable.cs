namespace Howest.Movies.Common.Extensions;

public static class Enumerable
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, int from, int size)
    {
        return source.Skip(from).Take(size);
    }
    
    public static IQueryable<T> ApplyPagination<T>(this IOrderedQueryable<T> source, int from, int size)
    {
        return source.Skip(from).Take(size);
    }
}