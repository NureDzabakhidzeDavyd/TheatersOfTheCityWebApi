using System.ComponentModel.DataAnnotations;

namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreatePerformanceRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Genre { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public string Language { get; set; }
    
    public IEnumerable<CreatePerformanceParticipantRequest> Participants { get; set; }
}