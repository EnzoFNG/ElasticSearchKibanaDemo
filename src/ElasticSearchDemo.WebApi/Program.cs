using ElasticSearchDemo.WebApi.Configurations;
using ElasticSearchDemo.WebApi.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuring Serilog
builder.Configuration.ConfigureLogging();
builder.Host.UseSerilog();

// Seed Data
InMemoryData.SeedData();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();