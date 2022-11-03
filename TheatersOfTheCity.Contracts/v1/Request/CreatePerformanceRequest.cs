namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreatePerformanceRequest
{
    public string Name { get; set; }
    
    public string Genre { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public string Language { get; set; }
    
    public IEnumerable<CreatePerformanceParticipantRequest> Participants { get; set; }
}