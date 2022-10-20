using System.ComponentModel.DataAnnotations;

namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreateContactRequest
{
    [Required]
    public string FirstName { get; set; }

    public string SecondName { get; set; }

    [Required]
    public DateTime Birth { get; set; }

    [Required]
    public  string Email { get; set; }
    
    [Required]
    public string Phone { get; set; }
}