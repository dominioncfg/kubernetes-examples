using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;

namespace KubernetesExample.BFF
{
    public class StudentsApiHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StudentsApiHealthCheck(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("StudentsBackendApi");
            var response = await httpClient.GetAsync("/ready");
          
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("A healthy result.");
            }

            return HealthCheckResult.Unhealthy("An unhealthy result.");
        }
    }

}
