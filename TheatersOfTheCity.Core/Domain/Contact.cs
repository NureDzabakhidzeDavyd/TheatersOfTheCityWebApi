using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using Dapper.Contrib.Extensions;
using TheatersOfTheCity.Core.Enums;

namespace TheatersOfTheCity.Core.Domain;

[Table("contact")]
public class Contact
{
    [Dapper.Contrib.Extensions.Key]
    public  int ContactId { get; set; }
    
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