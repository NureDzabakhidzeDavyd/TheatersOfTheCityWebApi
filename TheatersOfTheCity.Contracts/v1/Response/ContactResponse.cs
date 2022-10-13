using System.ComponentModel.DataAnnotations;

namespace TheatersOfTheCity.Contracts.v1.Response;

public class ContactResponse
{
    [Key]
    public  int ContactId { get; set; }
    
    public string FirstName { get; set; }

    public string SecondName { get; set; }

    public DateTime Birth { get; set; }

    public  string Email { get; set; }
    
    public string Phone { get; set; }
}