using Newtonsoft.Json;

namespace TheatersOfTheCity.Core.Domain;

public class UserProfile : BaseEntity
{
    [JsonProperty("id")]
    public override int Id { get; set; }
    
    [JsonProperty("name")]
    public string FullName { get; set; }
    
    [JsonProperty("given_name")]
    public string FirstName { get; set; }
    
    [JsonProperty("family_name")]
    public string SecondName { get; set; }
    
    [JsonProperty("picture")]
    public string Avatar { get; set; }
    
    [JsonProperty("locale")]
    public string Country { get; set; }
}