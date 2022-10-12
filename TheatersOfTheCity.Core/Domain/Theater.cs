using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;
[Dapper.Contrib.Extensions.Table("theater")]
public class Theater
{
    [Dapper.Contrib.Extensions.Key]
    public string TheaterId { get; set; }
    
    [Required]
    public  string Name { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public  string Address { get; set; }

    [Write(false)]
    public Contact Director { get; set; }

    public int DirectorId { get; set; }
}