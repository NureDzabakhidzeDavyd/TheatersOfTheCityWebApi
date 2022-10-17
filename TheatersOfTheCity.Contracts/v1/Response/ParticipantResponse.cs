using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Contracts.v1.Response;

public class ParticipantResponse
{
    public Contact Contact { get; set; }
    
    public string Role { get; set; }
}