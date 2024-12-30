using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;




namespace KubernetesExample.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext context;

    public SettingsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet()]
    [Route("all")]
    public Task<string> GetAll(CancellationToken cancellationToken)
    {
        var config = (_configuration as IConfigurationRoot).GetDebugView();
        return Task.FromResult(config);
    }

    [HttpGet()]
    [Route("host-info")]
    public Task<object> GetHostSettinsg(CancellationToken cancellationToken)
    {
        var result = new
        {
            HostName = Environment.MachineName,
            Os = Environment.OSVersion,
        };
        return Task.FromResult<object>(result);
    }


}


