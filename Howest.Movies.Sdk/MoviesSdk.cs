using Howest.Movies.Sdk.Abstractions;
using Howest.Movies.Sdk.Endpoints.Abstractions;
using Howest.Movies.Sdk.Stores;

namespace Howest.Movies.Sdk;

public class MoviesSdk : IMoviesSdk
{
    private PeriodicTimer _tokenRefreshTimer;
    
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ITokenStore _tokenStore;
    
    public const string HttpClientName = "MoviesSdkHttpClient";

    public IIdentityEndpoint Identity { get; }
    public IMovieEndpoint Movies { get; }
    public IReviewEndpoint Reviews { get; }
    public IGenreEndpoint Genres { get; }
    
    public MoviesSdk(ITokenStore tokenStore, IIdentityEndpoint identityEndpoint, IMovieEndpoint movieEndpoint, IReviewEndpoint reviewEndpoint, IGenreEndpoint genres)
    {
        _tokenRefreshTimer = new PeriodicTimer(TimeSpan.FromMinutes(60));
        _cancellationTokenSource = new CancellationTokenSource();

        _tokenStore = tokenStore;
        
        Identity = identityEndpoint;
        Identity.OnLogin += StartRefreshTokenTimerAsync;
        Identity.OnLogout += StopRefreshTokenTimer;
        Movies = movieEndpoint;
        Reviews = reviewEndpoint;
        Genres = genres;
    }
    
    private async Task StartRefreshTokenTimerAsync()
    {
        var token = await _tokenStore.GetTokenAsync();
        if (token is null)
            return;
        _tokenRefreshTimer.Period = TimeSpan.FromSeconds(Math.Max(token.ExpiresIn - 60, 1));
        _ = Task.Run(BackgroundRefreshToken);
    }
    
    private async Task StopRefreshTokenTimer()
    {
        await _cancellationTokenSource.CancelAsync();
        if (!_cancellationTokenSource.TryReset())
        {
            _tokenRefreshTimer.Dispose();
            _tokenRefreshTimer = new PeriodicTimer(TimeSpan.FromMinutes(60));
        }
    }
    
    private async Task BackgroundRefreshToken()
    {
        while (await _tokenRefreshTimer.WaitForNextTickAsync(_cancellationTokenSource.Token))
        {
            var result = await Identity.RefreshAsync(true);
            if (result.IsSuccess) continue;
            
            await Identity.LogoutAsync();
            await StopRefreshTokenTimer();
        }
    }
}