using Microsoft.EntityFrameworkCore;
using SkogsInsikt.Api.Middleware;
using SkogsInsikt.Application.Interfaces;
using SkogsInsikt.Application.Services;
using SkogsInsikt.Infrastructure.Data;
using SkogsInsikt.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SkogsInsiktDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IWeatherService, OpenMeteoWeatherService>();
builder.Services.AddScoped<ForestAnalysisService>();
builder.Services.AddScoped<IForestAreaService, ForestAreaService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
