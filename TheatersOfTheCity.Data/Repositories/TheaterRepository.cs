using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SqlKata;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class TheaterRepository : BaseRepository<Theater>, ITheaterRepository
{
    public TheaterRepository(RepositoryConfiguration sqlConfiguration,
        ILogger<BaseRepository<Theater>> logger) : base(sqlConfiguration, logger) {}

    public override async Task<IEnumerable<Theater>> GetAllAsync()
    {
        var query = new Query(nameof(Theater))
            .Join(nameof(Contact), nameof(Contact.ContactId), nameof(Theater.TheaterId));
        SqlResult compileResult = MySqlCompiler.Compile(query);
        string sql = compileResult.Sql;
        
        using var connection = new MySqlConnection(Connection);

        
        var result = await connection.QueryAsync<Theater, Contact, Theater>(sql, (theater, contact) =>
        {
            theater.Director = contact;
            return theater;
        });
        return result;
        // var sql =
        //     @"SELECT Theater.*, Contact.FirstName, Contact.SecondName 
        //     FROM Theater 
        //         LEFT JOIN Contact 
        //             ON Theater.ArtisticDirectorId = Contact.ContactId";
        //
    }

    public async override Task<IEnumerable<Theater>> CreateManyAsync(IEnumerable<Theater> entities)
    {
        throw new NullReferenceException();
    }
}