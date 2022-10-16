using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;

[Table(nameof(Scene))]
public class Scene
{
    /// <summary>
    /// Performance participant reference
    /// </summary>
    [Write(false)]
    public Participant Participant { get; set; }
    public int ParticipantId { get; set; }
    
    /// <summary>
    /// Performance in which participant take part
    /// </summary>
    [Write(false)]
    public Performance Performance { get; set; }
    public int PerformanceId { get; set; }
}