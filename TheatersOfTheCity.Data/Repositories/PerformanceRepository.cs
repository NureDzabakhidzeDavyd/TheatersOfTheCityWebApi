using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using SqlKata;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Data.Services;

namespace TheatersOfTheCity.Data.Repositories;

public class PerformanceRepository : BaseRepository<Performance>, IPerformanceRepository
{
    public PerformanceRepository(RepositoryConfiguration sqlConfiguration): base(sqlConfiguration) {}

    public override async Task<IEnumerable<Performance>> GetAllAsync()
    {
        // TODO: Update part 2
        var sceneTable = nameof(Scene);
        var performanceTable = TableName;
        
        var query = new Query(performanceTable)
            .LeftJoin(sceneTable,$"{sceneTable}.{nameof(Scene.PerformanceId)}", $"{performanceTable}.{nameof(Performance.PerformanceId)}")
            .LeftJoin(nameof(Contact), $"{nameof(Contact)}.{nameof(Contact.ContactId)}", $"{sceneTable}.{nameof(Scene.ParticipantId)}");
        var sql = query.MySqlQueryToString();
        var performances = await Connection.QueryAsync<Performance, Scene, Contact, Performance>(sql,
            (performance, scene, contact) =>
            {
                if (scene != null)
                {
                    performance.Participants.Add(scene);
                    scene.Participant = contact;
                    scene.Performance = new Lookup()
                    {
                        Id = performance.PerformanceId,
                        Name = performance.Name
                    };
                }
                return performance;
            }, splitOn: $"{nameof(Scene.SceneId)}, {nameof(Contact.ContactId)}");
        var result = performances.GroupBy(p => p.PerformanceId).Select(g =>
        {
            
            var groupPerformance = g.First();
            if (groupPerformance.Participants.Any())
            {
                groupPerformance.Participants = g.Select(p => p?.Participants.Single()).ToList();
            }
            return groupPerformance;
        });
        return result;
    }

    public override async Task<Performance> GetByIdAsync(int id)
    {
         var sceneTable = nameof(Scene);
         var contactTable = nameof(Contact);
         var performanceTable = TableName;
         
         var query = new Query(performanceTable)
             .LeftJoin(sceneTable,$"{sceneTable}.{nameof(Scene.PerformanceId)}", 
                 $"{performanceTable}.{nameof(Performance.PerformanceId)}")
             .LeftJoin(contactTable, $"{contactTable}.{nameof(Contact.ContactId)}",
                 $"{sceneTable}.{nameof(Scene.ParticipantId)}")
             .Where($"{performanceTable}.{nameof(Performance.PerformanceId)}", "=", id);
         var sql = query.MySqlQueryToString();
         
         var performances = (await Connection.QueryAsync<Performance, Scene, Contact, Performance>(sql,
             (performance, scene, contact) =>
             {
                 if (scene != null)
                 {
                     scene.Participant = contact;
                     scene.Performance = new Lookup()
                     {
                         Id = performance.PerformanceId,
                         Name = performance.Name
                     };
                     performance.Participants.Add(scene);
                 }
                 return performance;
             }, splitOn: $"{nameof(Scene.SceneId)}, {nameof(Contact.ContactId)}"));
         
         if (!performances.Any())
         {
             var result = await Connection.GetAsync<Performance>(id);
             return result;
         }
         
         var groupPerformance = performances.First();
         if (groupPerformance.Participants.Any())
         {
             groupPerformance.Participants = performances.Select(p => p?.Participants.Single()).ToList();
         }
             
         return groupPerformance;
    }
    
    public async Task<IEnumerable<Performance>> GetTheaterProgramsAsync(int Id)
    {
        var programTable = nameof(Program);
        var performanceTable = TableName;
    
        var query = new Query(performanceTable)
            .Join(programTable,$"{programTable}.{nameof(Program.PerformanceId)}", $"{performanceTable}.{nameof(Performance.PerformanceId)}")
            .Where($"{programTable}.{nameof(Program.TheaterId)}", "=", Id);
        var sql = query.MySqlQueryToString();
        var performances = await Connection.QueryAsync<Performance>(sql);

        return performances;
    }
}