using KubernetesExample.Settings;
using KubernetesExample.SharedDataStorage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("PgSql");
builder.Services.Configure<VersioningSettings>(
    builder.Configuration.GetSection(VersioningSettings.SectionName));
builder.Services.Configure<FailingSettings>(
    builder.Configuration.GetSection(FailingSettings.SectionName));


builder.Services.AddDbContextPool<AppDbContext>(opt =>
                opt.UseNpgsql(connectionString!,
                   o => o
                       .SetPostgresVersion(15, 0)
                )
           );
builder.Services.AddControllers();
builder.AddCustomHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(builder =>
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();
app.UseCustomHealthChecks();

app.Run();
