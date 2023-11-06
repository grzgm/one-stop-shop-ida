using Microsoft.EntityFrameworkCore;

namespace OneStopShopIdaBackend.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<UserItem> Users { get; set; } = null!;
        public DbSet<LunchRecurringItem> LunchRecurring { get; set; } = null!;
        public DbSet<LunchTodayItem> LunchToday { get; set; } = null!;
    }
}
