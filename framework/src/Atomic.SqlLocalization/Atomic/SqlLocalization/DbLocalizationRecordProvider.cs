using System.Collections.Generic;
using System.Linq;
using Atomic.Utils;

namespace Atomic.SqlLocalization
{
    public class DbLocalizationRecordProvider : ILocalizationRecordProvider
    {
        private readonly LocalizationDbContext _context;

        public DbLocalizationRecordProvider(LocalizationDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, string> GetAllLocalizations(string resourceKey)
        {
            Check.NotNull(resourceKey, nameof(resourceKey));

            lock (_context)
            {
                return _context.LocalizationRecords
                    .Where(record => record.ResourceKey == resourceKey)
                    .ToDictionary(
                        record => $"{record.Key}.{record.Culture}",
                        record => record.Value
                    );
            }
        }
    }
}