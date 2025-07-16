using Microsoft.Extensions.Diagnostics.HealthChecks;
using vidro.api.Persistance;

namespace vidro.api.Extension
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly VidroContext _context;

        public DatabaseHealthCheck(VidroContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Database.CanConnectAsync(cancellationToken);
                return HealthCheckResult.Healthy("Database connection is healthy");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database connection failed", ex);
            }
        }
    }
}
