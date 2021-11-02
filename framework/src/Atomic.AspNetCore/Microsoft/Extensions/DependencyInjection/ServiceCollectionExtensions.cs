using Atomic.AspNetCore.Users;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAtomicAspNetCore(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddTransient<ICurrentUser, CurrentUser>();
        }
    }
}