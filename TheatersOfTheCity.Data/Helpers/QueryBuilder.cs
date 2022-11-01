using SqlKata;
using TheatersOfTheCity.Core.Domain.Filters;
using TheatersOfTheCity.Core.Enums;

namespace TheatersOfTheCity.Data.Helpers;

public class QueryBuilder<T>
{
    private Query _query;

    public QueryBuilder(PaginationFilter paginationFilter, SortFilter? sortFilter, DynamicFilters? dynamicFilters, Query query = null)
    {
        var tableName = typeof(T).Name;
        _query = query is null ? AddQuery() : query;
        AddOptions(paginationFilter, sortFilter, dynamicFilters);
    }

    public void AddOptions(PaginationFilter paginationFilter, SortFilter? sortFilter, DynamicFilters? dynamicFilters)
    {
        if (sortFilter?.Field is not null)
        {
            AddSorting(sortFilter);
        }
        else if (dynamicFilters?.Filters is not null)
        {
            AddFilter(dynamicFilters);
        }
        AddPagination(paginationFilter);
    }

    private void Reset()
    {
        _query = AddQuery();
    }

    public Query Build()
    {
        var result = _query;
        Reset();
        return result;
    }

    private Query AddQuery()
    {
        var tableName = typeof(T).Name;
        var query = new Query(tableName);
        return query;
    }

    private void AddFilter(DynamicFilters? filters)
    {
        filters.Filters.ToList().ForEach(AddFilter);
    }

    private void AddFilter(DynamicFilter dynamicFilter)
    {
        switch (dynamicFilter.FieldType)
        {
            case (int)FieldType.Number:
            case (int)FieldType.Text:
                _query.Where(dynamicFilter.FieldName, "=", dynamicFilter.Value);
                break;
            default:
                throw new ArgumentException($"{dynamicFilter.FieldType} doesn't exist");
        }
    }

    private void AddSorting(SortFilter? sortFilter)
    {
        if (sortFilter.Descending)
        {
            _query.OrderByDesc(sortFilter.Field);
        }
        else
        {
            _query.OrderBy(sortFilter.Field);
        }
    }

    private void AddPagination(PaginationFilter paginationFilter)
    {
        _query
            .Skip((paginationFilter.Page - 1) * paginationFilter.Size)
            .Take(paginationFilter.Size);
    }
    
    public override string ToString()
    {
        var result = _query.MySqlQueryToString();
        Reset();
        return result;
    }
}