using System.Data;
using System.Reflection;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;
using TheatersOfTheCity.Core.Attributes;
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

    public async Task DeleteScenesByPerformanceIdAsync(int performanceId)
    {
        var contactTable = nameof(Contact);

        var query = new Query(TableName)
            .Where(nameof(Scene.PerformanceId), "=", performanceId)
            .AsDelete();
        var sql = query.MySqlQueryToString();

        await Connection.QueryAsync(sql);
    }

    public async Task<IEnumerable<Scene>> GetScenesByPerformanceIdAsync(int performanceId)
    {
        var contactTable = nameof(Contact);
        
        var query = new Query(TableName)
            .Where(nameof(Scene.PerformanceId), "=", performanceId)
            .Join(nameof(Contact), $"{contactTable}.{nameof(Contact.ContactId)}", $"{TableName}.{nameof(Scene.ParticipantId)}");
        var sql = query.MySqlQueryToString();

        var scenes = await Connection.QueryAsync<Scene, Contact, Scene>(sql, (scene, contact) =>
        {
            scene.Participant = contact;
            return scene;
        }, splitOn: nameof(Contact.ContactId));

        return scenes;
    }
    //
    // public async Task CreateSceneAsync(IEnumerable<Scene> scenes, int performanceId)
    // {
    //     // TODO: check props and work
    //     var data = scenes.Select(x => new object[] { x, performanceId }).ToArray();
    //
    //     var props = typeof(Scene).GetProperties()
    //         .Where(x => !Attribute.IsDefined(x, typeof(OrderAttribute)))
    //         .OrderBy(x => ((OrderAttribute)x
    //             .GetCustomAttributes(typeof(OrderAttribute), false)
    //             .Single()).Order);
    //     var columns =  props.Select(x => x.Name);
    //
    //     var query = new Query(TableName).AsInsert(scenes);
    //     var sql = query.MySqlQueryToString();
    //     await Connection.QueryAsync<Scene>(sql);
    // }
    //
    // public async Task UpdateSceneAsync(IEnumerable<int> participantsIds, int performanceId)
    // {
    //     var columns = new [] { "ParticipantId", "PerformanceId" };
    //     var data = participantsIds.Select(x => new object[] { x, performanceId }).ToArray();
    //     var query = new Query(TableName)
    //         .Where(nameof(Scene.PerformanceId), "=", performanceId)
    //         .AsDelete()
    //         .AsInsert(columns, data);
    //     
    //     var sql = query.MySqlQueryToString();
    //     await Connection.QueryAsync<Scene>(sql);
    // }
}