namespace TheatersOfTheCity.Contracts.v1.Request;

public class UpdateTheaterRequest
{
    public  string Name { get; set; }
    
    public string City { get; set; }
    
    public  string Address { get; set; }
    public int DirectorId { get; set; }
}