using KubernetesExample.SharedDataStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KubernetesExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly AppDbContext context;

        public StudentsController(ILogger<StudentsController> logger, AppDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        [HttpGet(Name = "GetAll")]
        public async Task<IEnumerable<Student>> GetAll(CancellationToken cancellationToken)
        {
            var students = await context.Students.ToListAsync(cancellationToken);
            return students;
        }
    }
}


