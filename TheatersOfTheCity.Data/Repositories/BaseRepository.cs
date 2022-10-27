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
using TheatersOfTheCity.Data.Services;

namespace TheatersOfTheCity.Data.Repositories;

public class BaseRepository<T> : IDisposable, IRepository<T> where T: class
{
    protected readonly IDbConnection Connection;
    protected readonly string TableName = typeof(T).Name;

    public BaseRepository(RepositoryConfiguration sqlConfiguration)
    {
        var connection = sqlConfiguration.DbConnection;
        Connection = new MySqlConnection(connection);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await Connection.InsertAsync(entity);

        return entity;
    }

    public async Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> entities)
    {
        await Connection.InsertAsync(entities);

        return entities;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        await Connection.UpdateAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        await Connection.DeleteAsync(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entityToDel = await Connection.GetAsync<T>(id);
        await Connection.DeleteAsync(entityToDel);
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        var result = await Connection.GetAsync<T>(id);
        return result;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var result = await Connection.GetAllAsync<T>();
        return result;
    }
    
    public async Task<IEnumerable<T>> GetManyByIdAsync(IEnumerable<int> ids, string columnName)
    {
        var columnId = columnName;
        var query = new Query(TableName).WhereIn(columnId, ids);
        var sql = query.MySqlQueryToString();
        
        var result = await Connection.QueryAsync<T>(sql);
        return result;
    }
    
    public async Task<bool> EntitiesAreExist(IEnumerable<int> ids, string idName)
    {
        var query = new Query(nameof(Contact)).WhereIn(idName, ids)
            .AsCount();
        var sql = query.MySqlQueryToString();
        
        var contactsCount = (await Connection.QueryAsync<int>(sql)).First();
        
        if (ids.Count() != contactsCount)
        {
            return false;
        }

        return true;
    }

    private void ReleaseUnmanagedResources()
    {
        Connection.Close();
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            Connection.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~BaseRepository()
    {
        Dispose(false);
    }
}