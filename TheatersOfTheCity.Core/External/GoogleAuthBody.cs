using System.Text.Json.Serialization;

namespace TheatersOfTheCity.Core.External;

public class GoogleAuthBody
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
    
    [JsonPropertyName("redirect_url")]
    public  string RedirectUrl { get; set; }
}