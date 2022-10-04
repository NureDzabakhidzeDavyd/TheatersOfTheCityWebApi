using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Extensions.Logging;
using TheatersOfTheCity.Business.External;
using TheatersOfTheCity.Business.Options;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Core.Services;
using TheatersOfTheCity.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog();
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var clientCredentials = builder.Configuration.GetSection(nameof(ClientCredentials)).Get<ClientCredentials>();
builder.Services.AddSingleton(clientCredentials);

builder.Services.AddSingleton<IGoogleService, GoogleService>();

builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();

#region Database services

var databaseConfiguration = builder.Configuration.GetSection(nameof(RepositoryConfiguration)).Get<RepositoryConfiguration>();
builder.Services.AddSingleton(databaseConfiguration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITheaterRepository, TheaterRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();

#endregion

builder.Services.AddEndpointsApiExplorer();

#region Swagger configuration

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "Theaters of the city API",
        Version = "v1",
        Title = "Theaters of the city"
    });
});

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();