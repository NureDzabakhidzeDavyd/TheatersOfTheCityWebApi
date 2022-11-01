namespace TheatersOfTheCity.Api.Installers;

public static class InstallerExtentions
{
    public static void InstallServicesInAsembly(this WebApplicationBuilder builder)
    {
        var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
        installers.ForEach(installer => installer.InstallService(builder.Services, builder.Configuration));
    }
}