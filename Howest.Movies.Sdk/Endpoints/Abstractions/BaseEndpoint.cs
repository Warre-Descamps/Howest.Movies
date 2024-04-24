namespace Howest.Movies.Sdk.Endpoints.Abstractions;

public abstract class BaseEndpoint
{
    private readonly IHttpClientFactory _httpClientFactory;
    public HttpClient HttpClient => _httpClientFactory.CreateClient(MoviesSdk.HttpClientName);
    
    public BaseEndpoint(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
}