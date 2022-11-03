using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;
[System.ComponentModel.DataAnnotations.Schema.Table(nameof(Program))]
public class Program
{ 
    [Key]
    public int ProgramId { get; set; }
    
    public int TheaterId { get; set; }
    [Write(false)]
    public List<Theater> Theaters { get; set; }

    public int PerformanceId { get; set; }
    [Write(false)]
    public List<Performance> Performances { get; set; }
}