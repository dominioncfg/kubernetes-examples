using Microsoft.Extensions.Diagnostics.HealthChecks;
using KubernetesExample;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

public static class HealthCheckExtensions
{
    private const string DumbHealthCheckTag = "Dumb";
    private const string SmartHealthCheckTag = "Smart";

    public static WebApplicationBuilder AddCustomHealthChecks(this WebApplicationBuilder builder)
    {
        builder
        .Services
            .AddHealthChecks()
                .AddCheck<DefaultHealthyCheck>("DefaultHealthyCheck", HealthStatus.Healthy, [DumbHealthCheckTag])
                .AddSqlServer(builder.Configuration.GetConnectionString("SqlServer")!, tags: ["Smart"]);

        return builder;
    }

    public static WebApplication UseCustomHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health/startup", new HealthCheckOptions()
        {
            ResponseWriter = HealthCheckWriters.WriteResponse,
        });

        app.MapHealthChecks("healthz", new HealthCheckOptions()
        {
            ResponseWriter = HealthCheckWriters.WriteResponse,
            Predicate = x => x.Tags.Any(x => x == DumbHealthCheckTag),
        });

        app.MapHealthChecks("ready", new HealthCheckOptions()
        {
            ResponseWriter = HealthCheckWriters.WriteResponse,
            Predicate = x => x.Tags.Any(x => x == DumbHealthCheckTag),
        });

        return app;
    }
}
