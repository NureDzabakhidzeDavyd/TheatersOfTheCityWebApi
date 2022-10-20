using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class TheaterRepository : BaseRepository<Theater>, ITheaterRepository
{
    public TheaterRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration) {}

    public override async Task<IEnumerable<Theater>> GetAllAsync()
    {
        var query = new Query(nameof(Theater)).Join(nameof(Contact), nameof(Contact.ContactId),
            nameof(Theater.DirectorId));
        string sql = QueryToString(query);

        using IDbConnection connection = new MySqlConnection(Connection);
        var result = (await connection.QueryAsync<Theater, Contact, Theater>(sql, (theater, contact) =>
        {
            theater.Director = contact;
            return theater;
        }, splitOn: nameof(Contact.ContactId)));
        return result;
    }

    public override async Task<Theater> GetByIdAsync(int id)
    {
        var query = new Query(nameof(Theater))
            .Join(nameof(Contact), nameof(Contact.ContactId),
            nameof(Theater.DirectorId));
        string sql = QueryToString(query);

        using IDbConnection connection = new MySqlConnection(Connection);
        var result = (await connection.QueryAsync<Theater, Contact, Theater>(sql, (theater, contact) =>
        {
            theater.Director = contact;
            return theater;
        }, splitOn: nameof(Contact.ContactId))).First();
        return result;
    }

    public async Task<Theater> GetTheaterByName(string name)
    {
        var query = new Query(nameof(Theater))
            .Where(nameof(Theater.Name), "=", name)
            .Join(nameof(Contact), nameof(Contact.ContactId),
                nameof(Theater.DirectorId));
        var sql = QueryToString(query);

        IDbConnection connection = new MySqlConnection(Connection);
        var result = await connection.QueryAsync<Theater, Contact, Theater>(sql, (theater, contact) =>
        {
            theater.Director = contact;
            return theater;
        }, splitOn: nameof(Contact.ContactId));
        return result.FirstOrDefault();
    }
}