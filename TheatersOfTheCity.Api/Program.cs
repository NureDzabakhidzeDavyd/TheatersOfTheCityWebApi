using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using Newtonsoft.Json;
using SqlKata.Compilers;
using TheatersOfTheCity.Api.Controllers.Extensions;
using TheatersOfTheCity.Business.External;
using TheatersOfTheCity.Business.Options;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Core.Services;
using TheatersOfTheCity.Data;
using TheatersOfTheCity.Data.Repositories;
using JwtConstants = System.IdentityModel.Tokens.Jwt.JwtConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var clientCredentials = builder.Configuration.GetSection(nameof(ClientCredentials)).Get<ClientCredentials>();
builder.Services.AddSingleton(clientCredentials);

var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddSingleton<IGoogleService, GoogleService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddValidatorsFromAssemblyContaining<TheatersOfTheCity.Core.Domain.Program>();

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(option =>
    {
        option.AllowAnyMethod();
        option.AllowAnyHeader();
        option.AllowAnyOrigin();
    });
});

#region Database services

var databaseConfiguration = builder.Configuration.GetSection(nameof(RepositoryConfiguration)).Get<RepositoryConfiguration>();
builder.Services.AddSingleton(databaseConfiguration);
builder.Services.AddScoped<ITheaterRepository, TheaterRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<ISeeder, Seeder>();

#endregion

builder.Services.AddEndpointsApiExplorer();

#region Swagger configuration

builder.Services.AddSwaggerGen(options =>
{
    options.MapType(typeof(TimeSpan), () => new OpenApiSchema
    {
        Type = "string",
        Format = "time",
        Example = new OpenApiString("00:00:00")
    });
    options.MapType(typeof(DateTime), () => new OpenApiSchema
    {
        Type = "string", 
        Format = "date",
        Example = new OpenApiString("2013-06-23")
    });
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "Theaters of the city API",
        Version = "v1",
        Title = "Theaters of the city"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        BearerFormat = "Bearer {jwt}",
        Description = "Please insert JWT into field",
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization"
    });

    var securityScheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {securityScheme, new string[] {} }
    });
    // xml comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = clientCredentials.ClientId,
        ValidateIssuer = true,
        
        ValidateAudience = false,
        RequireExpirationTime = true,
        
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(jwtSettings.Secret)),
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
    await seeder.Seed();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();
app.UseAuthentication();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();