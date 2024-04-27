using Howest.Movies.Dtos.Core;
using Newtonsoft.Json;

namespace Howest.Movies.Sdk.Extensions;

internal static class ResponseExtensions
{
    private static T Resolve<T>(T? result)
        where T : ServiceResult, new()
    {
        if (result is not null) return result;
        
        result = new T();
        result.Messages.Add(new ServiceMessage("NoContent", "No content found.", MessageType.Error));
        return result;
    }
    
    internal static async Task<T> ReadAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
        where T : ServiceResult, new()
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<T>(content);
        return Resolve(result);
    }

    internal static async Task<T> ReadAsync<T>(this Task<T?>? task, CancellationToken cancellationToken = default)
        where T : ServiceResult, new()
    {
        return task is not null
            ? Resolve(await task)
            : Resolve<T>(null);
    }
}