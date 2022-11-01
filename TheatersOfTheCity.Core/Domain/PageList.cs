namespace TheatersOfTheCity.Core.Domain;

public class PageList<T> : List<T>
{
    public int CurrentPage { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }

    public PageList(IEnumerable<T> data, int currentPage, int totalCount, int pageSize)
    {
        CurrentPage = currentPage;
        TotalCount = totalCount;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
        AddRange(data);
    }
}