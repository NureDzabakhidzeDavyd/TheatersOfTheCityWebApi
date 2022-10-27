using System.ComponentModel.DataAnnotations;

namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreateParticipantRequest
{
    public int ContactId { get; set; }
    
    public int PerformanceId { get; set; }
    
    public string Role { get; set; }
}