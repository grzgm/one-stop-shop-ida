using Microsoft.EntityFrameworkCore;
using OneStopShopIdaBackend.Models;

namespace OneStopShopIdaBackend.Services;

public partial class DatabaseService : DbContext
{
    public DatabaseService(DbContextOptions<DatabaseService> options) : base(options)
    {
    }

    public DbSet<UserItem> Users { get; set; } = null!;
    public DbSet<LunchRecurringItem> LunchRecurring { get; set; } = null!;
    public DbSet<LunchTodayItem> LunchToday { get; set; } = null!;

    private void IsDbSetNull(string dbSetName)
    {
        var property = GetType().GetProperty(dbSetName);
        if (property == null)
        {
            throw new InvalidOperationException($"Database  with name '{dbSetName}' not found.");
        }
    }
}