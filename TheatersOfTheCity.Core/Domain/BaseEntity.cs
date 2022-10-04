using Dapper.Contrib.Extensions;

namespace TheatersOfTheCity.Core.Domain;

public abstract class BaseEntity
{
    [Key]
    public virtual int Id { get; set; }
}