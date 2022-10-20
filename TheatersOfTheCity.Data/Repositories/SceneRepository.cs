using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class SceneRepository : BaseRepository<Scene>, ISceneRepository
{
    public SceneRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
    }

    public async Task CreateSceneAsync(IEnumerable<int> participantsIds, int performanceId)
    {
        var columns = new [] { "ParticipantId", "PerformanceId" };
        object[][] data = new object[participantsIds.Count()][];

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new object[] { participantsIds.ToArray()[i], performanceId };
        }
        
        var query = new Query(nameof(Scene)).AsInsert(columns, data);
        string sql = QueryToString(query);
        
        using IDbConnection connection = new MySqlConnection(Connection);
        await connection.QueryAsync<Scene>(sql);
    }

    public async Task UpdateSceneAsync(IEnumerable<int> participantsIds, int performanceId)
    {
        var columns = new [] { "ParticipantId", "PerformanceId" };
        var data = participantsIds.Select(x => new object[] { x, performanceId }).ToArray();
        var query = new Query(nameof(Scene))
            .Where(nameof(Scene.PerformanceId), "=", performanceId)
            .AsDelete()
            .AsInsert(columns, data);
        
        string sql = QueryToString(query);
        
        using IDbConnection connection = new MySqlConnection(Connection);
        await connection.QueryAsync<Scene>(sql);
    }
}