using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;
[System.ComponentModel.DataAnnotations.Schema.Table("Theater")]
public class Theater
{
    [Key]
    public int TheaterId { get; set; }
    
    public  string Name { get; set; }
    
    public string City { get; set; }
    
    public  string Address { get; set; }
    
    public  int DirectorId { get; set; }
    
    [Write(false)]
    public Contact Director { get; set; }
    
    [Write(false)]
    public IEnumerable<Performance> Performances { get; set; }
}