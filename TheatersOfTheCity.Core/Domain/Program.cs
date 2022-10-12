using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;

public class Program
{
    [ForeignKey("program_theater_theater_id_fk")]
    public int TheaterId { get; set; }
    [Write(false)]
    public Theater Theater { get; set; }
    
    [ForeignKey("program_performance_performance_id_fk")]
    public int PerformanceId { get; set; }
    [Write(false)]
    public Performance Performance { get; set; }
}