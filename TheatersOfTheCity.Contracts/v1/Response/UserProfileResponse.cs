namespace TheatersOfTheCity.Contracts.v1.Request;

public class UserProfileResponse
{
    public int UserId { get; set; }
    
    public string UserGoogleId { get; set; }
    
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
    
    public string Avatar { get; set; }
    
    public string Country { get; set; }
}