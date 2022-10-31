namespace TheatersOfTheCity.Core.Domain.Filters;

public class SortFilter
{
    public string? Field { get; set; } = null;

    public bool Descending { get; set; }
}