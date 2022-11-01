using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Contracts.Services;

public static class ToPageListService
{
    public static PageList<T> ToPageList<T>(this IEnumerable<T> data, int currentPage, int totalCount, int pageSize)
    {
        return new PageList<T>(data, currentPage, totalCount, pageSize);
    }
}