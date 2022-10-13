using System.ComponentModel.DataAnnotations;

namespace TheatersOfTheCity.Core.Domain;

public class Performance 
{
    [Key]
    public int PerformanceId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string Genre { get; set; }
    
    public string Duration { get; set; }

    public string Language { get; set; }
}