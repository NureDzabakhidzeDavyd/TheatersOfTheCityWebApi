namespace TheatersOfTheCity.Core.Domain.Filters;

public class PaginationFilter
{
    public int Page { get; set; } = 1;
    public int Size { get; set; }
}