using TheatersOfTheCity.Business.Options;
using TheatersOfTheCity.Core.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;


namespace TheatersOfTheCity.Business.External;

public class GoogleService : IGoogleService
{
    private readonly ClientCredentials _clientCredentials;

    public GoogleService(ClientCredentials clientCredentials)
    {
        _clientCredentials = clientCredentials;
    }
    
    
    public async Task<string> GoogleAuthUrlRequest()
    {
        var queryParams = new Dictionary<string, string>
        {
            { "client_id", _clientCredentials.ClientId },
            { "redirect_uri", "http://localhost:44330/GoogleOAuth/search" },
            {"scope", "https://www.googleapis.com/auth/userinfo.profile"},
            { "response_type", "code" },
            // {"code_challenge", },
            // {"code_challenge_method", "S256"},
        };

        var url = QueryHelpers.AddQueryString("https://accounts.google.com/o/oauth2/v2/auth", queryParams);
        return url;
    }
}