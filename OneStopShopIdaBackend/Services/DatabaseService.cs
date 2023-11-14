using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;
using WebPush;
using PushSubscription = OneStopShopIdaBackend.Models.PushSubscription;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService : DbContext
{
    private readonly VapidDetails _vapidDetails;
    public DatabaseService(DbContextOptions<DatabaseService> options, IConfiguration configuration) : base(options)
    {
        var vapidSubject = configuration.GetValue<string>("Vapid:Subject");
        var vapidPublicKey = configuration.GetValue<string>("Vapid:PublicKey");
        var vapidPrivateKey = configuration.GetValue<string>("Vapid:PrivateKey");

        CheckOrGenerateVapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);

        _vapidDetails = new VapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);
    }

    public DbSet<UserItem> Users { get; set; } = null!;
    public DbSet<LunchRecurringItem> LunchRecurring { get; set; } = null!;
    public DbSet<LunchTodayItem> LunchToday { get; set; } = null!;
    public DbSet<PushSubscription> PushSubscription { get; set; } = null!;

    private void IsDbSetNull(string dbSetName)
    {
        var property = GetType().GetProperty(dbSetName);
        if (property == null)
        {
            throw new InvalidOperationException($"Database  with name '{dbSetName}' not found.");
        }
    }

    public void CheckOrGenerateVapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey)
    {
        if (string.IsNullOrEmpty(vapidSubject) ||
            string.IsNullOrEmpty(vapidPublicKey) ||
            string.IsNullOrEmpty(vapidPrivateKey))
        {
            var vapidKeys = VapidHelper.GenerateVapidKeys();

            // Prints 2 URL Safe Base64 Encoded Strings
            Console.WriteLine($"Public {vapidKeys.PublicKey}");
            Console.WriteLine($"Private {vapidKeys.PrivateKey}");

            throw new Exception(
                "You must set the Vapid:Subject, Vapid:PublicKey and Vapid:PrivateKey application settings or pass them to the service in the constructor. You can use the ones just printed to the debug console.");
        }
    }

}