using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Contracts.v1.Response;

public class TheaterResponse
{
    public int TheaterId { get; set; }
    
    public  string Name { get; set; }
    
    public string City { get; set; }
    
    public  string Address { get; set; }

    public ContactResponse Director { get; set; }
}