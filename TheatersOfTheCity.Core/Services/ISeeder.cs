namespace TheatersOfTheCity.Core.Services;

public interface ISeeder
{
    public Task Seed(bool generateDatabase);
}