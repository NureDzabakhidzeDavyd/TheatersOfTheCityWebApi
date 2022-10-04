namespace TheatersOfTheCity.Core.Domain;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
}