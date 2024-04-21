using Howest.Movies.WebApi.Services.Abstractions;

namespace Howest.Movies.WebApi.Services.Background;

public class MyBackgroundService : BackgroundService
{
    private readonly PeriodicTimer _timer;
    private readonly IPosterManagementService _posterManagementService;

    public MyBackgroundService(IPosterManagementService posterManagementService)
    {
        _posterManagementService = posterManagementService;
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await _timer.WaitForNextTickAsync(stoppingToken))
        {
            _posterManagementService.CleanPosters();
        }
    }
}