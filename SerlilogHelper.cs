using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace ErrorLoggingWithKibana
{
    public static class SerlilogHelper
    {
    
        public static void ConfigureLogging()
        {
            var enviorment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                AddJsonFile(
                $"appsettings.{enviorment}.json", optional: true
                ).Build();
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .Enrich.WithExceptionDetails()
               .WriteTo.Debug()
               .WriteTo.Console()
               .WriteTo.Elasticsearch(ConfigureElasticSearchSink(configuration, enviorment))
               .Enrich.WithProperty("Environment", enviorment)
               .ReadFrom.Configuration(configuration)
               .CreateLogger();
        }
        private static ElasticsearchSinkOptions ConfigureElasticSearchSink(IConfigurationRoot configuration, string enviornment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,//Autmatically register
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{enviornment.ToLower()}-{DateTime.UtcNow:yyyyy-MM}",
                NumberOfReplicas = 1,
                NumberOfShards = 2,

            };
        }
    }

  
}
