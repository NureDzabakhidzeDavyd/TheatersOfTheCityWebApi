using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;

[System.ComponentModel.DataAnnotations.Schema.Table("Participant")]
public class Participant
{
    [System.ComponentModel.DataAnnotations.Key]
    public int ContactId { get; set; }
    [Write(false)]
    public Contact Contact { get; set; }
    
    public string Role { get; set; }
    
    [Write(false)]
    public Scene Scene { get; set; }
    public int SceneId { get; set; }
}