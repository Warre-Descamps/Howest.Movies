namespace Howest.Movies.Dtos.Core;

public class ServiceMessage
{
    public string Code { get; set; }
    public string Message { get; set; }
    public MessageType Type { get; set; }
    
    public ServiceMessage()
    {
    }

    public ServiceMessage(string message, MessageType type)
    {
        Code = type switch
        {
            MessageType.Error => nameof(MessageType.Error),
            MessageType.Info => nameof(MessageType.Info),
            MessageType.Warning => nameof(MessageType.Warning),
            _ => string.Empty
        };

        Type = type;
        Message = message;
    }

    public ServiceMessage(string code, string message, MessageType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public ServiceMessage(Exception exception)
    {
        Code = exception.GetType().Name;
        Message = exception.Message;
        Type = MessageType.Error;
    }
}