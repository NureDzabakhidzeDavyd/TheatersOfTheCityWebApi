using Dapper.Contrib.Extensions;
using TheatersOfTheCity.Core.Attributes;

namespace TheatersOfTheCity.Core.Domain;

[Table(nameof(Scene))]
public class Scene
{
    [Key]
    // TODO: SceneId isn't showed in perf rep when created new performance
    public int SceneId { get; set; }
    
    /// <summary>
    /// Performance participant reference
    /// </summary>
    [Write(false)]
    public Contact Participant { get; set; }
    
    public int ParticipantId { get; set; }
    
    /// <summary>
    /// Performance in which participant take part
    /// </summary>
    [Write(false)]
    public Lookup Performance { get; set; }
    public int PerformanceId { get; set; }
    
    public string Role { get; set; }
}