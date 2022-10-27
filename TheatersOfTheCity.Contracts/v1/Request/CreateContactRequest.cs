using System.ComponentModel.DataAnnotations;

namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreateContactRequest
{
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }
    
    public DateTime Birth { get; set; }

    public  string Email { get; set; }
    
    public string Phone { get; set; }
}