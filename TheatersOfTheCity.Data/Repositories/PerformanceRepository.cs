using Dapper;
using Dapper.Contrib.Extensions;
using SqlKata;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Domain.Filters;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Data.Helpers;

namespace TheatersOfTheCity.Data.Repositories;

public class PerformanceRepository : BaseRepository<Performance>, IPerformanceRepository
{
    private Query _getAllQuery;

    public PerformanceRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
        _getAllQuery = GetAllQuery();
    }
    
    public override async Task<IEnumerable<Performance>> GetAllAsync()
    {
        var query = GetAllQuery();
        var sql = query.MySqlQueryToString();
        var performances = await Connection.QueryAsync<Performance, Participant, Contact, Performance>(sql,
            (performance, participant, contact) =>
            {
                if (participant != null)
                {
                    performance.Participants.Add(participant);
                    participant.Contact = contact;
                    participant.Performance = new Lookup()
                    {
                        Id = performance.PerformanceId,
                        Name = performance.Name
                    };
                }
                return performance;
            }, splitOn: $"{nameof(Participant.ParticipantId)}, {nameof(Contact.ContactId)}");
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

    private Query GetAllQuery()
    {
        var participantTable = nameof(Participant);
        var performanceTable = TableName;

        var query = new Query(performanceTable)
            .LeftJoin(participantTable, $"{participantTable}.{nameof(Participant.PerformanceId)}",
                $"{performanceTable}.{nameof(Performance.PerformanceId)}")
            .LeftJoin(nameof(Contact), $"{nameof(Contact)}.{nameof(Contact.ContactId)}",
                $"{participantTable}.{nameof(Participant.ContactId)}");
        return query;
    }

    public override async Task<(IEnumerable<Performance> data, int count)> PaginateAsync(PaginationFilter paginationFilter, SortFilter? sortFilter, DynamicFilters? dynamicFilters)
    {
        var builder = new QueryBuilder<Performance>(paginationFilter, sortFilter, dynamicFilters, _getAllQuery);

        _getAllQuery = builder.Build();
        var data = await GetAllAsync();
        var count = await GetCount();
        _getAllQuery = GetAllQuery();
        return (data,count);
    }

    public override async Task<Performance> GetByIdAsync(int id)
    {
         var participantTable = nameof(Participant);
         var contactTable = nameof(Contact);
         var performanceTable = TableName;
         
         var query = new Query(performanceTable)
             .LeftJoin(participantTable,$"{participantTable}.{nameof(Participant.PerformanceId)}", 
                 $"{performanceTable}.{nameof(Performance.PerformanceId)}")
             .LeftJoin(contactTable, $"{contactTable}.{nameof(Contact.ContactId)}",
                 $"{participantTable}.{nameof(Participant.ContactId)}")
             .Where($"{performanceTable}.{nameof(Performance.PerformanceId)}", "=", id);
         var sql = query.MySqlQueryToString();
         
         var performances = (await Connection.QueryAsync<Performance, Participant, Contact, Performance>(sql,
             (performance, participant, contact) =>
             {
                 if (participant != null)
                 {
                     participant.Contact = contact;
                     participant.Performance = new Lookup()
                     {
                         Id = performance.PerformanceId,
                         Name = performance.Name
                     };
                     performance.Participants.Add(participant);
                 }
                 return performance;
             }, splitOn: $"{nameof(Participant.ParticipantId)}, {nameof(Contact.ContactId)}"));
         
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

    public async Task<IEnumerable<Lookup>> GetPerformanceShowsByIdAsync(int id)
    {
        var theaterTable = nameof(Theater);
        var programTable = nameof(Program);
        
        var query = new Query(theaterTable)
            .Join(programTable, $"{programTable}.{nameof(Program.TheaterId)}", $"{theaterTable}.{nameof(Theater.TheaterId)}")
            .Join(TableName, $"{TableName}.{nameof(Performance.PerformanceId)}", $"{programTable}.{nameof(Program.PerformanceId)}")
            .Where($"{theaterTable}.{nameof(Theater.TheaterId)}", "=", id)
            .Select(new[] {$"{theaterTable}.{nameof(Theater.TheaterId)}", $"{theaterTable}.{nameof(Theater.Name)}"});
        var sql = query.MySqlQueryToString();
        
        var theatersLookup = await Connection.QueryAsync<Theater, Lookup, Lookup>(sql,
            (theater, _) =>
            {
                var lookup = new Lookup()
                {
                    Id = theater.TheaterId,
                    Name = theater.Name
                };
                return lookup;
            }, splitOn: nameof(Theater.Name));

        return theatersLookup.Distinct();
    }

}