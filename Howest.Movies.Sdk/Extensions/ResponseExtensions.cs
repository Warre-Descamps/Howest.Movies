using Howest.Movies.Dtos.Core;
using Newtonsoft.Json;

namespace Howest.Movies.Sdk.Extensions;

internal static class ResponseExtensions
{
    internal static async Task<T> ReadAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default) where T : ServiceResult, new()
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<T>(content);
        if (result is null)
        {
            result = new T();
            result.Messages.Add(new ServiceMessage("NoContent", "No content found.", MessageType.Error));
        }
        return result;
    }
}