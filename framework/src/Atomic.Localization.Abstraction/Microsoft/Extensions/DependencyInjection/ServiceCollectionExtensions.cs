using Atomic.Localization.Abstraction;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLocalizationCore(this IServiceCollection services)
        {
            services.TryAddSingleton<IStringLocalizerFactory, AtomicStringLocalizerFactory>();
        }
    }
}