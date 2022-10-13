namespace TheatersOfTheCity.Contracts.v1.Request;

public class CreateContactRequest
{
    [System.ComponentModel.DataAnnotations.Required]
    public string FirstName { get; set; }

    public string SecondName { get; set; }

    [System.ComponentModel.DataAnnotations.Required]
    public DateTime Birth { get; set; }

    [System.ComponentModel.DataAnnotations.Required]
    public  string Email { get; set; }
    
    [System.ComponentModel.DataAnnotations.Required]
    public string Phone { get; set; }
}