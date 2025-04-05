using KubernetesExample.SharedDataStorage;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KubernetesExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions serializationOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public StudentsController(ILogger<StudentsController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this._httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "GetAll")]
        public async Task<IEnumerable<Student>> GetAll(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("StudentsBackendApi");
            var response = await httpClient.GetAsync("/Students");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            
            var result = JsonSerializer.Deserialize<List<Student>>(responseString, serializationOptions);
            return result ?? new List<Student>();
        }
    }
}


