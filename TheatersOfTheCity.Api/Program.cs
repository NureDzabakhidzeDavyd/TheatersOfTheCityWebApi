using FluentValidation;
using Serilog;
using TheatersOfTheCity.Api.Installers;
using TheatersOfTheCity.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddValidatorsFromAssemblyContaining<TheatersOfTheCity.Core.Domain.Program>();

builder.InstallServicesInAsembly();

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
app.UseResponseCaching();

app.UseAuthorization();
app.UseAuthentication();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();