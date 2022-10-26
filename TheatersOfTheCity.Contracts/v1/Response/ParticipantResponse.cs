using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Contracts.v1.Response;

public class ParticipantResponse
{
    public int ParticipantId { get; set; }

    public Contact Contact { get; set; }

    public Lookup Performance { get; set; }
    
    public string Role { get; set; }
}