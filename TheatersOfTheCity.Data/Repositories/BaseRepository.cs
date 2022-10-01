using System.Data;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using Dapper;
using MySql;
using MySql.Data.MySqlClient;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class BaseRepository<T> : IRepository<T> where T: class
{
    private readonly string _connectiong;
    private readonly string _table;
    
    public BaseRepository(MySqlRepositoryConfiguration sqlConfiguration)
    {
        _connectiong = sqlConfiguration.MySqlConnection;
        _table = sqlConfiguration.TableName;
    }

    public async Task<T> CreateAsync(T entity)
    {
        using IDbConnection connection = new MySqlConnection(_connectiong);
        var result = await connection.InsertAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        using IDbConnection connection = new MySqlConnection(_connectiong);
        await connection.UpdateAsync<T>(entity);
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        using IDbConnection connection = new MySqlConnection(_connectiong);
        await connection.DeleteAsync<T>(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        using IDbConnection connection = new MySqlConnection(_connectiong);
        var entityToDel = await connection.GetAsync<T>(id);
        await connection.DeleteAsync(entityToDel);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        using IDbConnection connection = new MySqlConnection(_connectiong);
        var result = await connection.GetAsync<T>(id);
        return result;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        using IDbConnection connection = new MySqlConnection(_connectiong);
        var result = await connection.GetAllAsync<T>();
        return result;
    }
}