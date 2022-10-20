using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Options;
using SqlKata;
using SqlKata.Compilers;
using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Data.Repositories;

public class BaseRepository<T> : IRepository<T> where T: class
{
    private readonly MySqlCompiler _mySqlCompiler;
    protected readonly string Connection;

    public BaseRepository(RepositoryConfiguration sqlConfiguration)
    {
        Connection = sqlConfiguration.DbConnection;
        _mySqlCompiler = new MySqlCompiler();
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

    public virtual async Task<T> GetByIdAsync(int id)
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

    /// <summary>
    /// Get many objects by ids
    /// </summary>
    /// <param name="ids">Ids enumerable for searching</param>
    /// <param name="columnName">Column used as search parameter</param>
    /// <returns>Enumerable of objects with given ids</returns>
    public virtual async Task<IEnumerable<T>> GetManyByIdAsync(IEnumerable<int> ids, string columnName)
    {
        var tableName = typeof(T).Name;
        var columnId = columnName;
        var query = new Query(tableName).WhereIn<int>(columnId, ids);
        string sql = QueryToString(query);
        
        using IDbConnection connection = new MySqlConnection(Connection);
        var result = await connection.QueryAsync<T>(sql);
        return result;
    }
    
    public async Task<bool> EntitiesAreExist(IEnumerable<int> ids, string idName)
    {
        var query = new Query(nameof(Contact)).WhereIn(idName, ids)
            .AsCount();
        var sql = QueryToString(query);
        
        using IDbConnection connection = new MySqlConnection(Connection);
        var contactsCount = (await connection.QueryAsync<int>(sql)).First();
        
        if (ids.Count() != contactsCount)
        {
            return false;
        }

        return true;
    }

    public string QueryToString(Query query)
    {
        var compiler = _mySqlCompiler;
        SqlResult sqlResult = compiler.Compile(query);
        return sqlResult.ToString();
    }
}