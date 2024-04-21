using System.Text.Json;
using Howest.Movies.Dtos.Core;

namespace Howest.Movies.Sdk.Extensions;

internal static class ResponseExtensions
{
    internal static async Task<T> ReadAsync<T>(this HttpResponseMessage response) where T : ServiceResult, new()
    {
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(content);
        if (result is null)
        {
            result = new T();
            result.Messages.Add(new ServiceMessage("NoContent", "No content found.", MessageType.Error));
        }
        return result;
    }
}