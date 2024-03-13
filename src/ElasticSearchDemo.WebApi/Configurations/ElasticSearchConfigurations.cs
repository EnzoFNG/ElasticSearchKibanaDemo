using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace ElasticSearchDemo.WebApi.Configurations;

public static class ElasticSearchConfigurations
{
    public static void ConfigureLogging(this IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        ArgumentNullException.ThrowIfNull(environment);

        var options = configuration.ConfigureElasticSink(environment);

        Log.Logger = new LoggerConfiguration()
         .Enrich.FromLogContext()
         .Enrich.WithExceptionDetails()
         .WriteTo.Debug()
         .WriteTo.Console()
         .WriteTo.Elasticsearch(options)
         .Enrich.WithProperty("Environment", environment)
         .ReadFrom.Configuration(configuration)
         .CreateLogger();
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(this IConfiguration configuration, string environment)
    {
        var elasticSearchConnectionString = configuration.GetConnectionString("ElasticSearchConnection");

        ArgumentNullException.ThrowIfNull(elasticSearchConnectionString);

        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        ArgumentNullException.ThrowIfNull(assemblyName);

        var dateTimeNowFormatted = $"{DateTime.UtcNow:yyyy-MM-dd}";
        var assemblyNameFormatted = assemblyName.ToLower().Replace(".", "-");

        var indexFormat = $"{assemblyNameFormatted}-{environment.ToLower()}-{dateTimeNowFormatted}";

        return new ElasticsearchSinkOptions(new Uri(elasticSearchConnectionString))
        {
            AutoRegisterTemplate = true,
            IndexFormat = indexFormat,
            NumberOfReplicas = 2,
            NumberOfShards = 2
        };
    }
}