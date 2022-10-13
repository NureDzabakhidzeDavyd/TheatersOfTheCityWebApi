using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Options;
using SqlKata;
using SqlKata.Compilers;

namespace TheatersOfTheCity.Data.Repositories;

public class BaseRepository<T> : IRepository<T> where T: class
{
    private readonly ILogger<BaseRepository<T>> _logger;
    protected readonly MySqlCompiler MySqlCompiler;
    protected readonly string Connection;

    protected BaseRepository(RepositoryConfiguration sqlConfiguration, ILogger<BaseRepository<T>> logger)
    {
        Connection = sqlConfiguration.DbConnection;
        _logger = logger;
        MySqlCompiler = new MySqlCompiler();
    }

    public async Task<T> CreateAsync(T entity)
    {
        using IDbConnection connection = new MySqlConnection(Connection);
        await connection.InsertAsync(entity);

        return entity;
    }

    public virtual async Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> entities)
    {
        using IDbConnection connection = new MySqlConnection(Connection);
        await connection.InsertAsync(entities);
        
        return entities;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        using IDbConnection connection = new MySqlConnection(Connection);
        await connection.UpdateAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        using IDbConnection connection = new MySqlConnection(Connection);
        await connection.DeleteAsync(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        using IDbConnection connection = new MySqlConnection(Connection);
        var entityToDel = await connection.GetAsync<T>(id);
        await connection.DeleteAsync(entityToDel);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        using IDbConnection connection = new MySqlConnection(Connection);
        var result = await connection.GetAsync<T>(id);
        return result;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        using IDbConnection connection = new MySqlConnection(Connection);
        var result = await connection.GetAllAsync<T>();
        return result;
    }
    
}