using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;

[Table("Performance")]
public class Performance 
{
    [Key]
    public int PerformanceId { get; set; }
    
    public string Name { get; set; }
    // One genre and one language
    public string Genre { get; set; }
    
    public TimeSpan Duration { get; set; }

    public string Language { get; set; }

    [Write(false)] 
    public List<Participant> Participants { get; set; } = new List<Participant>();

}