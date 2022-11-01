using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Api.Controllers.Extensions;

public static class PageListHeaderResponseService
{
    public static dynamic PageListHeaderResponse<T>(PageList<T> pageList)
    {
            var metadata = new
            {
                pageList.TotalCount,
                pageList.PageSize,
                pageList.CurrentPage,
                pageList.TotalPages,
            };

            return metadata;
    }
}