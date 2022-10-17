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

    public async Task CreateScene(IEnumerable<int> participantsIds, int performanceId)
    {
        var columns = new [] { "ParticipantId", "PerformanceId" };
        object[][] data = new object[participantsIds.Count()][];
        // participantsIds.ToList().ForEach(x => data.Add(new object[] {x, performanceId}));

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new object[] { participantsIds.ToArray()[i], performanceId };
        }
        
        var compiler = new MySqlCompiler();
        var query = new Query(nameof(Scene)).AsInsert( columns, data);
        SqlResult sqlResult = compiler.Compile(query);
        string sql = sqlResult.ToString();
        
        using IDbConnection connection = new MySqlConnection(Connection);
        await connection.QueryAsync<Scene>(sql);
    }
}