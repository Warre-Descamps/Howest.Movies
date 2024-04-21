using Howest.Movies.Dtos.Core.Abstractions;

namespace Howest.Movies.Dtos.Core;

public class ServiceResult
{
    public List<ServiceMessage> Messages { get; set; }
    public virtual bool IsSuccess => Messages.All(m => m.Type != MessageType.Error);

    public ServiceResult()
    {
        Messages = new List<ServiceMessage>();
    }

    public ServiceResult(params ServiceMessage[] messages) : this()
    {
        Messages = [..messages];
    }

    public object GetReturn(IReturnResolver resolver)
    {
        return resolver.Resolve(this);
    }
}
     
public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }
    public override bool IsSuccess => base.IsSuccess && Data is not null;

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