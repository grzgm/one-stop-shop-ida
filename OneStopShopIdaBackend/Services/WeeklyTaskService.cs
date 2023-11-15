using System;

namespace OneStopShopIdaBackend.Services;

public class WeeklyTaskService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public WeeklyTaskService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {

        // Get the current time
        var now = DateTime.Now;

        var timeUntilNextFriday = GetNextFriday() - now;

        // Schedule the timer to run every week (7 days)
        _timer = new Timer(DoWork, null, timeUntilNextFriday, TimeSpan.FromDays(7));

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
            await scopedProcessingService.SendLunchRecurring();
        }
        Console.WriteLine("--------------------------------------------");
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
    private static DateTime GetNextFriday()
    {
        var day = DayOfWeek.Friday;
         var date = DateTime.Now;
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int daysToAdd = ((int)day - (int)date.DayOfWeek + 7) % 7;

        if (daysToAdd == 0)
        {
            daysToAdd = 7;
        }
        return new DateTime(date.Year, date.Month, date.Day, 14, 0, 0).AddDays(daysToAdd);
    }
}
