using Microsoft.EntityFrameworkCore;

namespace OneStopShopIdaBackend.Models
{
    public class SessionEntryContext : DbContext
    {
        public SessionEntryContext(DbContextOptions<SessionEntryContext> options)
            : base(options)
        {
        }

        public DbSet<SessionEntryItem> SessionEntryItems { get; set; } = null!;
    }
}
