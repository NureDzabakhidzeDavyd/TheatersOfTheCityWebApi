using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;
[System.ComponentModel.DataAnnotations.Schema.Table("Theater")]
public class Theater
{
    [System.ComponentModel.DataAnnotations.Key]
    public string TheaterId { get; set; }
    
    public  string Name { get; set; }
    
    public string City { get; set; }
    
    public  string Address { get; set; }

    [Write(false)]
    public Contact Director { get; set; }
}