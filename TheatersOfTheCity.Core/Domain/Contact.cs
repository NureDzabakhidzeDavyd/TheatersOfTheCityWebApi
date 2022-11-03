using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;

[Table("Contact")]
public class Contact
{
    [Key]
    public  int ContactId { get; set; }
    
    public string FirstName { get; set; }

    public string SecondName { get; set; }

    public DateTime Birth { get; set; }

    public  string Email { get; set; }
    
    public string Phone { get; set; }
}