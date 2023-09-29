using ErrorLoggingWithKibana;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

SerlilogHelper.ConfigureLogging();
builder.Host.UseSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//void configureLogging()
//{
//    var enviorment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
//    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json",optional:false, reloadOnChange:true).
//        AddJsonFile(
//        $"appsettings.{enviorment}.json", optional:true
//        ).Build();
//     Log.Logger=new LoggerConfiguration()
//        .Enrich.FromLogContext()
//        .WriteTo.Debug()
//        .WriteTo.Console()
//        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration,enviorment))
//        .Enrich.WithProperty("Environment",enviorment)
//        .ReadFrom.Configuration(configuration)
//        .CreateLogger();
//}

//ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string enviornment)
//{
//    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
//    {
//        AutoRegisterTemplate = true,//Autmatically register
//        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{enviornment.ToLower()}-{DateTime.UtcNow:yyyyy-MM}",
//        NumberOfReplicas = 1,
//        NumberOfShards = 2,

//    };
//}