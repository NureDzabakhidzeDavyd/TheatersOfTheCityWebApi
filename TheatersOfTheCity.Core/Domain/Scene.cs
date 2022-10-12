using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;

[Dapper.Contrib.Extensions.Table("scene")]
public class Scene
{
    /// <summary>
    /// Performance participant reference
    /// </summary>
    [Write(false)]
    [ForeignKey("scene_participant_scene_id_fk")]
    public Participant Participant { get; set; }
    public int ParticipantId { get; set; }
    
    /// <summary>
    /// Performance in which participant take part
    /// </summary>
    [Write(false)]
    public Performance Performance { get; set; }
    [ForeignKey("scene_performance_actor_id_fk")]
    public int PerformanceId { get; set; }
}