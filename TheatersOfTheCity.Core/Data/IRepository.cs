namespace TheatersOfTheCity.Core.Data;

public interface IRepository<T>
{
    public Task<T> CreateAsync(T entity);
    public Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> entities);
    
    public Task<T> UpdateAsync(T entity);

    public Task DeleteAsync(T entity);
    public Task DeleteByIdAsync(int id);
    
    public Task<T> GetByIdAsync(int id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<IEnumerable<T>> GetManyById(IEnumerable<int> ids, string columnName);

}