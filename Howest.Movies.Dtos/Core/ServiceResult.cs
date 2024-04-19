namespace Howest.Movies.Dtos.Core;

public class ServiceResult
{
    public List<ServiceMessage> Messages { get; set; }
    public bool IsSuccess => Messages.All(m => m.Type != MessageType.Error);

    public ServiceResult()
    {
        Messages = new List<ServiceMessage>();
    }

    public ServiceResult(params ServiceMessage[] messages) : this()
    {
        Messages = [..messages];
    }
}
     
public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }

    public ServiceResult() : base()
    {
    }

    public ServiceResult(T data) : this()
    {
        Data = data;
    }

    public ServiceResult(params ServiceMessage[] messages) : base(messages)
    {
        Data = default;
    }

    public static implicit operator ServiceResult<T>(T value) => new(value);
}