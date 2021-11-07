using Atomic.SqlLocalization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlLocalization(this IServiceCollection services)
        {
            services.TryAddSingleton<IStringLocalizerFactory, AtomicStringLocalizerFactory>();
            services.AddSingleton<ILocalizationRecordProvider, DbLocalizationRecordProvider>();
        }
    }
}