using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using Newtonsoft.Json;
using SqlKata.Compilers;
using TheatersOfTheCity.Api.Controllers.Extensions;
using TheatersOfTheCity.Api.Installers;
using TheatersOfTheCity.Business.External;
using TheatersOfTheCity.Business.Options;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain.Filters;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Core.Services;
using TheatersOfTheCity.Data;
using TheatersOfTheCity.Data.Repositories;
using JwtConstants = System.IdentityModel.Tokens.Jwt.JwtConstants;

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