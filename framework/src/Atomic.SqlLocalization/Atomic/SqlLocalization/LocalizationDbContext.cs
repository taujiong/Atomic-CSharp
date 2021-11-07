using Microsoft.EntityFrameworkCore;

namespace Atomic.SqlLocalization
{
    public class LocalizationDbContext : DbContext
    {
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options) : base(options)
        {
        }

        public DbSet<LocalizationRecord> LocalizationRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LocalizationRecord>().HasKey(d => d.Id);
            builder.Entity<LocalizationRecord>().HasAlternateKey(d => new { d.ResourceKey, d.Culture });
        }
    }
}