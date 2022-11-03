using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace TheatersOfTheCity.Core.Domain;

[Table("User")]
public class UserProfile
{
    public int UserId { get; set; }
    
    [JsonProperty("id")]
    public string UserGoogleId { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("given_name")]
    public string FirstName { get; set; }
    
    [JsonProperty("family_name")]
    public string SecondName { get; set; }
    
    
    [JsonProperty("picture")]
    public string Avatar { get; set; }
    
    [JsonProperty("locale")]
    public string Country { get; set; }
    
    [JsonIgnore]
    public string RefreshToken { get; set; }
}