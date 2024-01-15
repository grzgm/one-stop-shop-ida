using OneStopShopIdaBackend.Models;
using OneStopShopIdaBackend.Services;

namespace OneStopShopIdaBackend.Controllers;

public class LunchDaysTaskController : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer;

    public LunchDaysTaskController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Trigger the daily task every day at 9:00
        var now = DateTime.UtcNow;
        var midnight = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0, DateTimeKind.Utc);
        var timeUntilMidnight = midnight.AddHours(24) - now;

        _timer = new Timer(DoWork, null, timeUntilMidnight, TimeSpan.FromDays(1));
        // _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        Notification notification = new()
        {
            Title = "iDA Lunch Reminder",
            Body = "Open the App and register for lunch in next week!",
            Actions = new List<NotificationAction>() { new() { Action = "register", Title = "register" } }
        };

        using (var scope = _serviceProvider.CreateScope())
        {
            var databaseService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
            List<UserItem> userItems = (await databaseService.GetUserItems()).ToList();
            foreach (var userItem in userItems)
            {
                try
                {
                    LunchDaysItem lunchDaysItem =
                        await databaseService.GetRegisteredDays(userItem.MicrosoftId);
                    
                    if (lunchDaysItem.IsRegisteredOnDate(DateTime.Now.AddDays(1)))
                    {
                        await databaseService.SendNotificationsToUser(notification, userItem);
                    }
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("No User in Database");
                }
                catch (Exception)
                {
                    Console.WriteLine("Cannot send lunch reminder");
                }
            }
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
