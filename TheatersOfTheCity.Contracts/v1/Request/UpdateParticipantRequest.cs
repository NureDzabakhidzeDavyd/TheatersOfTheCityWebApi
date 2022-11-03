namespace TheatersOfTheCity.Contracts.v1.Request;

public class UpdateParticipantRequest
{
    public int PerformanceId { get; set; }
    
    public int ContactId { get; set; }
    
    public string Role { get; set; }
}