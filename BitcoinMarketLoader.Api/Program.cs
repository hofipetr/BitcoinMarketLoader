using BitcoinMarketLoader.Application.Extensions;
using BitcoinMarketLoader.Api.BackgroundServices;
using BitcoinMarketLoader.Infrastructure.Extensions;
using Microsoft.OpenApi;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var nlogConfigFile = builder.Environment.IsDevelopment()
    ? "NLog.Development.config"
    : "NLog.config";

builder.Logging.ClearProviders();
builder.Logging.AddNLogWeb(nlogConfigFile);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bitcoin Market Loader API",
        Version = "v1",
        Description = "API for retrieving Bitcoin market ticks and currency exchange rates.",
    });

    var xmlDocumentationFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDocumentationFile));
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddHostedService<MarketTickPollingBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bitcoin Market Loader API v1");
        options.DocumentTitle = "Bitcoin Market Loader API";
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
