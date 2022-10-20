using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Data.Services;

namespace TheatersOfTheCity.Data.Repositories;

public class SceneRepository : BaseRepository<Scene>, ISceneRepository
{
    public SceneRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
    }

    public async Task CreateSceneAsync(IEnumerable<int> participantsIds, int performanceId)
    {
        var columns = new [] { "ParticipantId", "PerformanceId" };
        var data = participantsIds.Select(x => new object[] { x, performanceId }).ToArray();

        var query = new Query(TableName).AsInsert(columns, data);
        var sql = query.MySqlQueryToString();
        await Connection.QueryAsync<Scene>(sql);
    }

    public async Task UpdateSceneAsync(IEnumerable<int> participantsIds, int performanceId)
    {
        var columns = new [] { "ParticipantId", "PerformanceId" };
        var data = participantsIds.Select(x => new object[] { x, performanceId }).ToArray();
        var query = new Query(TableName)
            .Where(nameof(Scene.PerformanceId), "=", performanceId)
            .AsDelete()
            .AsInsert(columns, data);
        
        var sql = query.MySqlQueryToString();
        await Connection.QueryAsync<Scene>(sql);
    }
}