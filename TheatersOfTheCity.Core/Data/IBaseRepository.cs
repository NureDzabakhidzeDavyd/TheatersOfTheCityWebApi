namespace TheatersOfTheCity.Core.Data;

public interface IBaseRepository<T>
{
    public Task<T> CreateAsync(string[] values);
    public Task<T> UpdateAsync();

    public Task DeleteAsync(T entity);
    public Task DeleteByIdAsync(int id);
    
    public Task<T> GetByIdAsync(int id);
    public Task<IEnumerable<T>> GetAllAsync();
}