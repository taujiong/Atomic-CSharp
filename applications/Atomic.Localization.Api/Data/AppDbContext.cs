using Atomic.Localization.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Atomic.Localization.Api.Data
{
    public class AppDbContext : LocalizationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}