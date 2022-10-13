using System.ComponentModel.DataAnnotations.Schema;

namespace TheatersOfTheCity.Core.Domain;

[Table("scene")]
public class Scene
{
    /// <summary>
    /// Performance participant reference
    /// </summary>
    [NotMapped]
    [ForeignKey("scene_participant_scene_id_fk")]
    public Participant Participant { get; set; }
    public int ParticipantId { get; set; }
    
    /// <summary>
    /// Performance in which participant take part
    /// </summary>
    [NotMapped]
    public Performance Performance { get; set; }
    [ForeignKey("scene_performance_actor_id_fk")]
    public int PerformanceId { get; set; }
}