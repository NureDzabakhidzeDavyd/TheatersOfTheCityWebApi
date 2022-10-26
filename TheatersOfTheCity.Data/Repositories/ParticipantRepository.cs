using System.Data;
using System.Reflection;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.Attributes;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Data.Services;

namespace TheatersOfTheCity.Data.Repositories;

public class ParticipantRepository : BaseRepository<Participant>, IParticipantRepository
{
    public ParticipantRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
    }

    public async Task DeleteParticipantsByPerformanceIdAsync(int performanceId)
    {
        var contactTable = nameof(Contact);

        var query = new Query(TableName)
            .Where(nameof(Participant.PerformanceId), "=", performanceId)
            .AsDelete();
        var sql = query.MySqlQueryToString();

        await Connection.QueryAsync(sql);
    }

    public async Task<IEnumerable<Participant>> GetParticipantsByPerformanceIdAsync(int performanceId)
    {
        var contactTable = nameof(Contact);
        
        var query = new Query(TableName)
            .Where(nameof(Participant.PerformanceId), "=", performanceId)
            .Join(nameof(Contact), $"{contactTable}.{nameof(Contact.ContactId)}", $"{TableName}.{nameof(Participant.ContactId)}");
        var sql = query.MySqlQueryToString();

        var participants = await Connection.QueryAsync<Participant, Contact, Participant>(sql, (participant, contact) =>
        {
            participant.Contact = contact;
            return participant;
        }, splitOn: nameof(Contact.ContactId));

        return participants;
    }

    public async override Task<IEnumerable<Participant>> GetAllAsync()
    {
        // TODO: Update part 2
        var performanceTable = nameof(Performance);
        var contactTable = nameof(Contact);
        
        var query = new Query(TableName)
            .Join(performanceTable,$"{performanceTable}.{nameof(Performance.PerformanceId)}", $"{TableName}.{nameof(Participant.PerformanceId)}")
            .Join(nameof(Contact), $"{nameof(Contact)}.{nameof(Contact.ContactId)}", $"{TableName}.{nameof(Participant.ContactId)}");
        var sql = query.MySqlQueryToString();
        var participants = await Connection.QueryAsync<Participant, Performance, Contact, Participant>(sql,
            (participant, performance, contact) =>
            {
                participant.Performance = new Lookup() 
                    {
                        Id = performance.PerformanceId, 
                        Name = performance.Name
                        
                    };
                participant.Contact = contact;
                return participant;
            }, splitOn: $"{nameof(Performance.PerformanceId)}, {nameof(Contact.ContactId)}");

        return participants;
    }

    public override async Task<Participant> GetByIdAsync(int id)
    {
        var contactTable = nameof(Contact);
        var performanceTable = nameof(Performance);
         
        var query = new Query(TableName)
            .Join(performanceTable,$"{performanceTable}.{nameof(Performance.PerformanceId)}", 
                $"{TableName}.{nameof(Participant.PerformanceId)}")
            .Join(contactTable, $"{contactTable}.{nameof(Contact.ContactId)}",
                $"{TableName}.{nameof(Participant.ContactId)}")
            .Where($"{TableName}.{nameof(Participant.ParticipantId)}", "=", id);
        var sql = query.MySqlQueryToString();
         
        var participant = await Connection.QueryAsync<Participant, Performance, Contact, Participant>(sql,
            (participant, performance, contact) =>
            {
               
                    participant.Contact = contact;
                    participant.Performance = new Lookup()
                    {
                        Id = performance.PerformanceId,
                        Name = performance.Name
                    };
                return participant;
            }, splitOn: $"{nameof(Performance.PerformanceId)}, {nameof(Contact.ContactId)}");

        return participant.First();
    }

    public async Task<IEnumerable<Participant>> GetParticipantsByContactId(int id)
    {
        var contactTable = nameof(Contact);
        var performanceTable = nameof(Performance);
         
        var query = new Query(TableName)
                .Join(performanceTable,$"{performanceTable}.{nameof(Performance.PerformanceId)}", 
                    $"{TableName}.{nameof(Participant.PerformanceId)}")
                .Join(contactTable, $"{contactTable}.{nameof(Contact.ContactId)}",
                    $"{TableName}.{nameof(Participant.ContactId)}")
            .Where($"{TableName}.{nameof(Participant.ContactId)}", "=", id);
        var sql = query.MySqlQueryToString();
        
        var participants = await Connection.QueryAsync<Participant, Performance, Contact, Participant>(sql,
            (participant, performance, contact) =>
            {
               
                participant.Contact = contact;
                participant.Performance = new Lookup()
                {
                    Id = performance.PerformanceId,
                    Name = performance.Name
                };
                return participant;
            }, splitOn: $"{nameof(Performance.PerformanceId)}, {nameof(Contact.ContactId)}");

        return participants;
    }
}