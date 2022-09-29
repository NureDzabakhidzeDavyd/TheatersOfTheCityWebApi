namespace TheatersOfTheCity.Contracts.v1.Request;

public class GoogleAuthCodeResponse
{
    public string AccessToken { get; set; }
    
    public  string RefreshToken { get; set; }
}