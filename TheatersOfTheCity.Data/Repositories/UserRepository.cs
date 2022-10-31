using System.Data.Common;
using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SqlKata;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Data.Helpers;

namespace TheatersOfTheCity.Data.Repositories;

public class UserRepository : BaseRepository<UserProfile>, IUserRepository
{
    public UserRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration) {}

    public async Task<UserProfile?> GetUserByEmail(string email)
    {
        var query = new Query("User").Where(nameof(UserProfile.Email), "=", email);
        var sql = query.MySqlQueryToString();
        var result = await Connection.QuerySingleOrDefaultAsync<UserProfile?>(sql, new {email});
        return result;
    }
}