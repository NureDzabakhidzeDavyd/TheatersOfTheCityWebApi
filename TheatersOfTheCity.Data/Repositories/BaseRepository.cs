using System.Data;
using Dapper;
using MySql;
using MySql.Data.MySqlClient;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T: class
{
    private readonly string _connectiong;
    private readonly string _table;
    private readonly string _entityProperties = typeof(T).GetProperties().ToString() ?? throw new InvalidOperationException();
    
    public BaseRepository(MySqlRepositoryConfiguration sqlConfiguration)
    {
        _connectiong = sqlConfiguration.MySqlConnection;
        _table = sqlConfiguration.TableName;
    }

    public async Task<T> CreateAsync(string[] values)
    {
        var query = $"INSERT INTO {_table} ({_entityProperties}) VALUES ({values})";
        using IDbConnection connection = new MySqlConnection(_connectiong);
        var result = await connection.QueryFirstAsync(query, new {values});
        return result;
    }

    public async Task<T> UpdateAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var query = $"DELETE FROM {_table} WHERE Id = @id";
        using IDbConnection connection = new MySqlConnection(_connectiong);
        await connection.ExecuteAsync(query, new {id});
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var query = $"SELECT * FROM {_table} WHERE Id = @id";
        using IDbConnection connection = new MySqlConnection(_connectiong);
        var result = await connection.QueryFirstAsync(query);

        return result;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var query = $"SELECT * From {_table}";
        using IDbConnection connection = new MySqlConnection(_connectiong);
        var result = await connection.QueryAsync<T>(query);

        return result;
    }
}