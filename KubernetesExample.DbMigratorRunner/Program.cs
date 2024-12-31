using KubernetesExample.SharedDataStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = CreateHost(args);

using var dbContext = GetDbContext(host);
await dbContext.Database.MigrateAsync();

await host.RunAsync();



static IHost CreateHost(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .UseConsoleLifetime()
        .ConfigureServices((context, services) =>
        {
            var connectionString = context.Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString));
        })
        .ConfigureAppConfiguration((context, builder) =>
        {
        })
        .Build();
}

static AppDbContext GetDbContext(IHost host)
{
    var scope = host.Services.CreateScope();
    return scope.ServiceProvider.GetRequiredService<AppDbContext>();
}