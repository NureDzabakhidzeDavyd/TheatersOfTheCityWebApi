using System.ComponentModel.DataAnnotations.Schema;
using Dapper;

namespace TheatersOfTheCity.Core.Domain;
[Table(nameof(Program))]
public class Program
{
    [ForeignKey("program_theater_theater_id_fk")]
    public int TheaterId { get; set; }

    [ForeignKey("program_performance_performance_id_fk")]
    public int PerformanceId { get; set; }
}