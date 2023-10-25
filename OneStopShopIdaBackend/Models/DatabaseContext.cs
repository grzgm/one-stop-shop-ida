using Microsoft.EntityFrameworkCore;

namespace OneStopShopIdaBackend.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LunchItem>()
                .HasKey(e => new { e.MicrosoftId, e.DayName });

            // Other configurations
        }
        public DbSet<UserItem> Users { get; set; } = null!;
        public DbSet<LunchItem> Lunch { get; set; } = null!;
    }
}
