namespace TheatersOfTheCity.Contracts.v1.Request;

public class UserProfileResponse
{
    public string id { get; set; }
    
    public string FullName { get; set; }
    
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
    
    public string Avatar { get; set; }
    
    public string Country { get; set; }
}