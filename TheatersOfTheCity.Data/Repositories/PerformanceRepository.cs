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
        var participantTable = nameof(Participant);
        var sceneTable = nameof(Scene);
        var performanceTable = TableName;
        
        var query = new Query(performanceTable)
            .LeftJoin(sceneTable,$"{sceneTable}.{nameof(Scene.PerformanceId)}", $"{performanceTable}.{nameof(Performance.PerformanceId)}")
            .LeftJoin(participantTable, $"{participantTable}.{nameof(Participant.ContactId)}", $"{sceneTable}.{nameof(Scene.ParticipantId)}")
            .LeftJoin(nameof(Contact), $"{nameof(Contact)}.{nameof(Contact.ContactId)}", $"{participantTable}.{nameof(Participant.ContactId)}");
        var sql = query.MySqlQueryToString();
        var performances = await Connection.QueryAsync<Performance, Participant, Contact, Performance>(sql,
            (performance, participant, contact) =>
            {
                if (participant != null)
                {
                    performance.Participants.Add(participant);
                    participant.Contact = contact;
                }
                return performance;
            }, splitOn: $"{nameof(Participant.ContactId)}, {nameof(Participant.ContactId)}");
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
        var participantTable = TableName;
        var sceneTable = nameof(Scene);
        var contactTable = nameof(Contact);
        var performanceTable = nameof(Performance);
        
        var query = new Query(performanceTable)
            .Join(sceneTable,$"{sceneTable}.{nameof(Scene.PerformanceId)}", 
                $"{performanceTable}.{nameof(Performance.PerformanceId)}")
            .Join(participantTable, $"{participantTable}.{nameof(Participant.ContactId)}",
                $"{sceneTable}.{nameof(Scene.ParticipantId)}")
            .Join(contactTable, $"{contactTable}.{nameof(Contact.ContactId)}",
                $"{participantTable}.{nameof(Participant.ContactId)}")
            .Where($"{performanceTable}.{nameof(Performance.PerformanceId)}", "=", id);
        var sql = query.MySqlQueryToString();
        
        var performances = (await Connection.QueryAsync<Performance, Participant, Contact, Performance>(sql,
            (performance, participant, contact) =>
            {
                if (participant != null)
                {
                    participant.Contact = contact;
                    performance.Participants.Add(participant);
                }
                return performance;
            }, splitOn: $"{nameof(Contact.ContactId)}"));

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
    
    public async Task<IEnumerable<Lookup>> GetTheaterProgramsAsync(int Id)
    {
        var programsLookup = new List<Lookup>();
        
        var programTable = nameof(Program);
        var performanceTable = TableName;

        var query = new Query(performanceTable)
            .Join(programTable,$"{programTable}.{nameof(Program.PerformanceId)}", $"{performanceTable}.{nameof(Performance.PerformanceId)}")
            .Where($"{programTable}.{nameof(Program.TheaterId)}", "=", Id);
        var sql = query.MySqlQueryToString();
        await Connection.QueryAsync<Performance, Program, Performance>(sql,
            (performance, _) =>
            {
                var lookup = new Lookup()
                {
                    Id = performance.PerformanceId,
                    Name = performance.Name
                };
                programsLookup.Add(lookup);
                return performance;
            }, splitOn: $"{nameof(Program.PerformanceId)}");

        if (!programsLookup.Any())
        {
            return null;
        }
        
        return programsLookup;
    }
}