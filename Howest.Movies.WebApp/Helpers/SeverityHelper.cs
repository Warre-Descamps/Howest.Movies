using Howest.Movies.Dtos.Core;
using MudBlazor;

namespace Howest.Movies.WebApp.Helpers;

public static class SeverityHelper
{
    public static Severity GetSeverityClass(MessageType type) => type switch
    {
        MessageType.Error => Severity.Error,
        MessageType.Info => Severity.Info,
        MessageType.Warning => Severity.Warning,
        _ => Severity.Normal
    };
}