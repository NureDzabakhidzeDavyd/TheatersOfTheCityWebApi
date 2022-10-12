using System.Data.Common;
using System.Net.Mail;
using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
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
        // var sql =
        //     @"SELECT Theater.*, Contact.FirstName, Contact.SecondName 
        //     FROM Theater 
        //         LEFT JOIN Contact 
        //             ON Theater.ArtisticDirectorId = Contact.ContactId";
        //
        // using DbConnection connection = new MySqlConnection(Connection);
        // var result = await connection.QueryAsync<Theater, Contact, Theater>(sql, (theater, contact) =>
        // {
        //     theater.ArtisticDirector = contact;
        //     return theater;
        // });
        // return result;
        throw new NullReferenceException();
    }
}