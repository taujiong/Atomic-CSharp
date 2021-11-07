using Atomic.Localization.Abstraction;
using Atomic.Localization.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlLocalization(this IServiceCollection services)
        {
            services.AddLocalizationCore();
            services.AddSingleton<ILocalizationRecordProvider, DbLocalizationRecordProvider>();
        }
    }
}