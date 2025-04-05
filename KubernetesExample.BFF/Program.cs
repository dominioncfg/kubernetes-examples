using KubernetesExample.BFF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.AddCustomHealthChecks();

builder.Services.AddHttpClient("StudentsBackendApi", httpClient =>
{
    var url= builder.Configuration["Backends:StudentsApi"];
    httpClient.BaseAddress = new Uri(url!);
});


var app = builder.Build();

app.UseCors(builder =>
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseCustomHealthChecks();

app.Run();
