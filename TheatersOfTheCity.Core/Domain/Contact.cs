using TheatersOfTheCity.Core.Enums;

namespace TheatersOfTheCity.Core.Domain;

public class Contact : BaseEntity
{
    public string FullName { get; set; }

    public DateTime Birth { get; set; }

    public Position Position { get; set; }

    public  string Email { get; set; }
    
    public string Phone { get; set; }
}