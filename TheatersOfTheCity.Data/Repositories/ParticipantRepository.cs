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
using TheatersOfTheCity.Core.Domain.Filters;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Data.Helpers;

namespace TheatersOfTheCity.Data.Repositories;

public class ParticipantRepository : BaseRepository<Participant>, IParticipantRepository
{
    private Query _getAllQuery;
    
    public ParticipantRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
        _getAllQuery = GetAllQuery();
    }

    public async Task DeleteParticipantsByPerformanceIdAsync(int performanceId)
    {
        var contactTable = nameof(Contact);

        var query = new Query(TableName)
            .Where(nameof(Participant.PerformanceId), "=", performanceId)
            .AsDelete().MySqlQueryToString();

        await Connection.QueryAsync(query);
    }

    public async Task<IEnumerable<Participant>> GetParticipantsByPerformanceIdAsync(int performanceId)
    {
        var contactTable = nameof(Contact);
        
        var query = new Query(TableName)
            .Where(nameof(Participant.PerformanceId), "=", performanceId)
            .Join(nameof(Contact), $"{contactTable}.{nameof(Contact.ContactId)}", 
                $"{TableName}.{nameof(Participant.ContactId)}")
            .MySqlQueryToString();

        var participants = await Connection.QueryAsync<Participant, Contact, Participant>(query, (participant, contact) =>
        {
            participant.Contact = contact;
            return participant;
        }, splitOn: nameof(Contact.ContactId));

        return participants;
    }

    public override async Task<(IEnumerable<Participant> data, int count)> PaginateAsync(PaginationFilter paginationFilter, SortFilter? sortFilter, DynamicFilters? dynamicFilters)
    {
        var builder = new QueryBuilder<Participant>(paginationFilter, sortFilter, dynamicFilters, _getAllQuery);

        _getAllQuery = builder.Build();
        var data = await GetAllAsync();
        var count = await GetCount();
        _getAllQuery = GetAllQuery();
        return (data, count);
    }

    public override async Task<IEnumerable<Participant>> GetAllAsync()
    {
        var query = GetAllQuery().MySqlQueryToString();
        
        var participants = await Connection.QueryAsync<Participant, Performance, Contact, Participant>(query,
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

    private Query GetAllQuery(string paginateQuery = null)
    {
        var performanceTable = nameof(Performance);

        var query = new Query(TableName)
            .Join(performanceTable, $"{performanceTable}.{nameof(Performance.PerformanceId)}",
                $"{TableName}.{nameof(Participant.PerformanceId)}")
            .Join(nameof(Contact), $"{nameof(Contact)}.{nameof(Contact.ContactId)}",
                $"{TableName}.{nameof(Participant.ContactId)}");

        return query;
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