namespace TheatersOfTheCity.Core.Domain.Filters;

public class DynamicFilter
{
    public string FieldName { get; set; }
    public int FieldType { get; set; }
    public string Value { get; set; }
}
public class DynamicFilters
{
    public IEnumerable<DynamicFilter>? Filters { get; set; } = null;
}