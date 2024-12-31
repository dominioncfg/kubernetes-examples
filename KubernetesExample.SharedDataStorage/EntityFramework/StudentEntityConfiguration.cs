using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace KubernetesExample.SharedDataStorage;

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
    }
}
