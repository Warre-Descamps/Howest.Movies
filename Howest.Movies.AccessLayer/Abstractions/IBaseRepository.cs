namespace Howest.Movies.AccessLayer.Abstractions;

public interface IBaseRepository<T, in TKey>
{
    Task<IList<T>> FindAsync();
    Task<T?> GetByIdAsync(TKey id);
    Task<T> AddAsync(T obj);
    Task<T?> UpdateAsync(TKey id, T obj);
    Task DeleteAsync(TKey id);
}