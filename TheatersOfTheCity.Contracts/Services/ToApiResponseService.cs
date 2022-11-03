using TheatersOfTheCity.Contracts.Common;

namespace TheatersOfTheCity.Contracts.Services;

public static class ToApiResponseService
{
    public static ApiResponse<T> ToApiResponse<T>(this T response, string? errorMessage = null)
    {
        var isSuccess = errorMessage is null ? true : false;
        return new ApiResponse<T>()
        {
            ErrorMessage = errorMessage,
            Result = response,
            IsSuccess = isSuccess
        };
    }
}