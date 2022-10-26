using TheatersOfTheCity.Contracts.v1.Response;
using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Contracts.v1.Request;

public class PerformanceResponse
{
    public int PerformanceId { get; set; }
    
    public string Name { get; set; }
    
    public string Genre { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public string Language { get; set; }
    
    public IEnumerable<SceneResponse> Participants { get; set; }
}