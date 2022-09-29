using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.External;

namespace TheatersOfTheCity.Core.Services;

public interface IGoogleService
{
    public Task<GoogleTokenBody> RefreshAccessToken(string refreshToken);

    public Task<GoogleTokenBody> GetAccessTokenAsync(string code);

    public Task<UserProfile> GetUserProfile(string accessToken);
}