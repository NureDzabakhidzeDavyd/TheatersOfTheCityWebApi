using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatersOfTheCity.Core.Domain;

[Table("participant")]
public class Participant
{
    [Key]
    public int ContactId { get; set; }
    
    public Contact Contact { get; set; }
    
    public string Role { get; set; }
    
    [NotMapped]
    public Scene Scene { get; set; }
    [ForeignKey("performance_participant_contact_fk")]
    public int SceneId { get; set; }
}