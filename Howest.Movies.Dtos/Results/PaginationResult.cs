namespace Howest.Movies.Dtos.Results;

public class PaginationResult<T>
{
    public T Items { get; set; }
    public int From { get; set; }
    public int Size { get; set; }
    
    public PaginationResult()
    {
    }
    
    public PaginationResult(T data, int from, int size)
    {
        Items = data;
        From = from;
        Size = size;
    }
}