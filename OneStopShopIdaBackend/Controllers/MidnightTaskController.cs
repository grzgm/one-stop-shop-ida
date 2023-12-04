using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

public class MidnightTaskController : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public MidnightTaskController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Trigger the daily task every day at midnight
        var now = DateTime.UtcNow;
        var midnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        var timeUntilMidnight = midnight.AddHours(24) - now;

        _timer = new Timer(DoWork, null, timeUntilMidnight, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
            await scopedProcessingService.UpdateAllLunchTodayItems(false);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
