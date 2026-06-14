using BitcoinMarketLoader.Application.Extensions;
using BitcoinMarketLoader.Infrastructure.Extensions;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

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
