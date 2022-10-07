using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Contracts.v1.Response;

public class TheaterResponse
{
    public  string Name { get; set; }
    
    public string City { get; set; }
    
    public  string Address { get; set; }
    
    public string Email { get; set; }
    
    public string Phohe { get; set; }
    
    public Contact ArtisticDirector { get; set; }
}