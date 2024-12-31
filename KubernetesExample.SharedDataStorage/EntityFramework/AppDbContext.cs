using Microsoft.EntityFrameworkCore;

namespace KubernetesExample.SharedDataStorage;
public class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentEntityConfiguration).Assembly);
    }
}
