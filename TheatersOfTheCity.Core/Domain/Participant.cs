using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;

[Dapper.Contrib.Extensions.Table("participant")]
public class Participant
{
    [Dapper.Contrib.Extensions.Key]
    public int ContactId { get; set; }
    
    public Contact Contact { get; set; }
    
    public string Role { get; set; }
    
    [Write(false)]
    public Scene Scene { get; set; }
    [ForeignKey("performance_participant_contact_fk")]
    public int SceneId { get; set; }
}