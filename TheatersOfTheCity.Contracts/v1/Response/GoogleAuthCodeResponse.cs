namespace TheatersOfTheCity.Contracts.v1.Request;

public class GoogleAuthCodeResponse
{
    public string Code { get; set; }
    
    public  string RedirectUrl { get; set; }
}