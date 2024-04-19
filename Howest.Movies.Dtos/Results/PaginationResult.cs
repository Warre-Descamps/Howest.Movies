namespace Howest.Movies.Dtos.Results;

public class PaginationResult<T>
{
    public T Data { get; set; }
    public int From { get; set; }
    public int Size { get; set; }
    
    public PaginationResult(T data, int from, int size)
    {
        Data = data;
        From = from;
        Size = size;
    }
}