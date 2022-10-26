namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreateSceneRequest
{
    public int ParticipantId { get; set; }
    
    public string Role { get; set; }
}