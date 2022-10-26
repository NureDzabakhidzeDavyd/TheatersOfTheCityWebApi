using Dapper.Contrib.Extensions;
using TheatersOfTheCity.Core.Attributes;

namespace TheatersOfTheCity.Core.Domain;

[Table(nameof(Participant))]
public class Participant
{
    [Key]
    // TODO: SceneId isn't showed in perf rep when created new performance
    public int ParticipantId { get; set; }
    
    [Write(false)]
    public Contact Contact { get; set; }
    
    public int ContactId { get; set; }
    
    [Write(false)]
    public Lookup Performance { get; set; }
    public int PerformanceId { get; set; }
    
    public string Role { get; set; }
}