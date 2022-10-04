namespace TheatersOfTheCity.Core.Domain;

public class ApiErrorResponse<T> : ApiResponse<T>
{
    public  string Error { get; set; }
}