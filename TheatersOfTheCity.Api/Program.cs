using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Extensions.Logging;
using TheatersOfTheCity.Business.External;
using TheatersOfTheCity.Business.Options;
using TheatersOfTheCity.Core.Services;

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


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "Theaters of the city API",
        Version = "v1",
        Title = "Theaters of the city"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();