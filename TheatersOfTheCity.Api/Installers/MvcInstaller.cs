using Microsoft.AspNetCore.Mvc;

namespace TheatersOfTheCity.Api.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers(options =>
        {
            options.CacheProfiles.Add("120SecondsDuration",
                new CacheProfile()
                {
                    Duration = 30
                });
        });
        services.AddHttpClient();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(option =>
            {
                option.AllowAnyMethod();
                option.AllowAnyHeader();
                option.AllowAnyOrigin();
            });
        });

        services.AddResponseCaching();

        services.AddEndpointsApiExplorer();
    }
}