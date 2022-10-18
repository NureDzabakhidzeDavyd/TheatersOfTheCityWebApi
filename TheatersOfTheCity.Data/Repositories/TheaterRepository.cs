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
        var query = new Query(nameof(Theater)).Join(nameof(Contact), nameof(Contact.ContactId),
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

    public async Task<IEnumerable<Performance>> GetTheaterProgramsAsync(int Id)
    {
        // select  p.* from Performance p inner join Program pr on pr.PerformanceId = p.PerformanceId
        // inner join Theater t on pr.TheaterId = t.TheaterId
        // where t.TheaterId = id
        var theaterTable = nameof(Theater);
        var programTable = nameof(Program);
        var performanceTable = nameof(Performance);
        
        var query = new Query(performanceTable)
            .Join(programTable,$"{programTable}.{nameof(Program.PerformanceId)}", $"{performanceTable}.{nameof(Performance.PerformanceId)}")
            .Join(theaterTable, $"{theaterTable}.{nameof(Theater.TheaterId)}", $"{programTable}.{nameof(Program.TheaterId)}")
            .Where($"{programTable}.{nameof(Program.TheaterId)}", "=", Id);
        var sql = QueryToString(query);
        // TODO: Make inner join with participants to get contacts and create TheaterProgramResponse
        using IDbConnection connection = new MySqlConnection(Connection);
        var result = await connection.QueryAsync<Performance>(sql);
        return result;
    }
}