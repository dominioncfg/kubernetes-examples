using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

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

public class Student
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentEntityConfiguration).Assembly);
    }
}

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Age);

        builder.HasData(new Student()
        {
            Id = Guid.Parse("b8881257-533d-4cee-a037-bfd1cedb5fa4"),
            Name = "Perico",
            Age = 18,
        });
        //
    }
}

