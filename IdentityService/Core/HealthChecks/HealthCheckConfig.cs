using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.APP.Core.HealthChecks
{
    public static class HealthCheckConfig
    {
        public static IServiceCollection AddHealthChecksConfig(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}