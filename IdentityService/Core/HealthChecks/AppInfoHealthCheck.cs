using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.APP.Core.HealthChecks
{
    public class AppInfoHealthCheck : IHealthCheck
    {
        private readonly IWebHostEnvironment env;

        public AppInfoHealthCheck(IWebHostEnvironment env)
        {
            this.env = env;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var result = await Task.Run(() =>
            {
                var version = GetType().Assembly.GetName().Version.ToString();
                var envName = env.EnvironmentName;

                return (version, envName);
            });

            return HealthCheckResult.Healthy($"Application version: {result.version} - Environment: {result.envName}");
        }
    }
}
