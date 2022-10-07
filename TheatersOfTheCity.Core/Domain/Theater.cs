using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;
[Table("Theater")]
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
    
    public string Email { get; set; }
    
    [Required]
    public string Phone { get; set; }
    
    public Contact ArtisticDirector { get; set; }
}