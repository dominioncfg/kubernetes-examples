using KubernetesExample.Settings;
using KubernetesExample.SharedDataStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KubernetesExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly AppDbContext _context;
        private readonly IOptions<VersioningSettings> _versionSettings;

        public StudentsController(ILogger<StudentsController> logger, AppDbContext context, IOptions<VersioningSettings> versionSettings)
        {
            _logger = logger;
            _context = context;
            _versionSettings = versionSettings;
        }

        [HttpGet(Name = "GetAll")]
        public async Task<IEnumerable<Student>> GetAll(CancellationToken cancellationToken)
        {
            var students = await _context.Students.ToListAsync(cancellationToken);
            if (_versionSettings.Value.OverrideName)
            {
                students = students.Select(x => new Student()
                {
                    Id = x.Id,
                    Name = x.Name + " (Modified)",
                    Age = x.Age,
                }).ToList();
            }
            return students;
        }
    }
}


