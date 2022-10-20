using System.ComponentModel.DataAnnotations;

namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreateParticipantRequest
{
    [Required]
    public int ContactId { get; set; }
    
    [Required]
    public string Role { get; set; }
}