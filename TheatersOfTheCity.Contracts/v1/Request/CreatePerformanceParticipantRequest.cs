namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreatePerformanceParticipantRequest
{
    public int ContactId { get; set; }

    public string Role { get; set; }
}