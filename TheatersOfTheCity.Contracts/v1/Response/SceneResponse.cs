using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Contracts.v1.Response;

public class SceneResponse
{
    public int SceneId { get; set; }

    public Contact Participant { get; set; }

    public Lookup Performance { get; set; }
    
    public string Role { get; set; }
}