using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace TheatersOfTheCity.Api.Installers;

public class SwaggerInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
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
    }
}