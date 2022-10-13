namespace TheatersOfTheCity.Contracts.v1.Request;

public class UpdateContactRequest
{
    public string FirstName { get; set; }

    public string SecondName { get; set; }

    public DateTime Birth { get; set; }

    public  string Email { get; set; }
    
    public string Phone { get; set; }
}