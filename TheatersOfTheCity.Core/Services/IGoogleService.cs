namespace TheatersOfTheCity.Core.Services;

public interface IGoogleService
{
    public Task<string> GoogleAuthUrlRequest();
}