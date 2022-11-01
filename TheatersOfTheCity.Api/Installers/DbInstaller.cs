using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Core.Services;
using TheatersOfTheCity.Data;
using TheatersOfTheCity.Data.Repositories;

namespace TheatersOfTheCity.Api.Installers;

public class DbInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration = configuration.GetSection(nameof(RepositoryConfiguration)).Get<RepositoryConfiguration>();
        services.AddSingleton(databaseConfiguration);
        services.AddScoped<ITheaterRepository, TheaterRepository>();
        services.AddScoped<IPerformanceRepository, PerformanceRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProgramRepository, ProgramRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<ISeeder, Seeder>();
    }
}