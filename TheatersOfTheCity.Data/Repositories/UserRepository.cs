using System.Data.Common;
using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class UserRepository : BaseRepository<UserProfile>, IUserRepository
{
    public UserRepository(RepositoryConfiguration sqlConfiguration, 
        ILogger<BaseRepository<UserProfile>> logger) : base(sqlConfiguration, logger) {}

    public async Task<UserProfile?> GetUserByEmail(string email)
    {
        var command = @"SELECT * FROM User WHERE email = @email";
        using DbConnection connection = new MySqlConnection(Connection);
        var result = await connection.QuerySingleOrDefaultAsync<UserProfile?>(command, new {email});
        return result;
    }
}