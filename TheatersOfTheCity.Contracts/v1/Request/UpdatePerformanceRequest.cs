namespace TheatersOfTheCity.Contracts.v1.Request;

public class UpdatePerformanceRequest
{
    public string Name { get; set; }
    
    public string Genre { get; set; }
    
    public DateTime Duration { get; set; }
    
    public string Language { get; set; }
}