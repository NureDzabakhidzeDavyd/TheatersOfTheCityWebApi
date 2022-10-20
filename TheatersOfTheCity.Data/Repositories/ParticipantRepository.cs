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

public class ParticipantRepository : BaseRepository<Participant>, IParticipantRepository
{
    public ParticipantRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
    }
    
    // public override async Task<IEnumerable<ParticipantLookup>> GetAllAsync()
    // {
    //     var participantTable = TableName;
    //     var contactTable = nameof(Contact);
    //     var performanceTable = nameof(Performance);
    //     var sceneTable = nameof(Performance);
    //
    //     
    //     var query = new Query(participantTable)
    //         .Join(contactTable, $"{contactTable}.{nameof(Contact.ContactId)}", $"{participantTable}.{nameof(Participant.ContactId)}")
    //         .Join(sceneTable, $"{sceneTable}.{nameof(Scene.ParticipantId)}", $"{participantTable}.{nameof(Participant.ContactId)}")
    //         .Join(performanceTable, $"{performanceTable}.{nameof(Performance.PerformanceId)}", $"{sceneTable}.{nameof(Scene.PerformanceId)}");
    //
    //     var sql = query.MySqlQueryToString();
    //     var result = await Connection.QueryAsync<ParticipantLookup, Contact, Performance, ParticipantLookup>(sql,
    //         (participant, contact, performance) =>
    //         {
    //             participant.Contact = contact;
    //             participant.Performance = new Lookup()
    //             {
    //                 Id = performance.PerformanceId,
    //                 Name = performance.Name
    //             };
    //             return participant;
    //         }, splitOn: $"{nameof(Contact.ContactId)}, {nameof(Performance.PerformanceId)}");
    //    
    //     return result;
    // }
    //
    // public override async Task<ParticipantLookup> GetByIdAsync(int id)
    // {
    //     var participantTable = TableName;
    //     var contactTable = nameof(Contact);
    //     var performanceTable = nameof(Performance);
    //     var sceneTable = nameof(Performance);
    //
    //     
    //     var query = new Query(participantTable)
    //         .Join(contactTable, $"{contactTable}.{nameof(Contact.ContactId)}", $"{participantTable}.{nameof(Participant.ContactId)}")
    //         .Join(sceneTable, $"{sceneTable}.{nameof(Scene.ParticipantId)}", $"{participantTable}.{nameof(Participant.ContactId)}")
    //         .Join(performanceTable, $"{performanceTable}.{nameof(Performance.PerformanceId)}", $"{sceneTable}.{nameof(Scene.PerformanceId)}");
    //
    //     var sql = query.MySqlQueryToString();
    //     var result = await Connection.QueryAsync<ParticipantLookup, Contact, Performance, ParticipantLookup>(sql,
    //         (participant, contact, performance) =>
    //         {
    //             participant.Contact = contact;
    //             participant.Performance = new Lookup()
    //             {
    //                 Id = performance.PerformanceId,
    //                 Name = performance.Name
    //             };
    //             return participant;
    //         }, splitOn: $"{nameof(Contact.ContactId)}, {nameof(Performance.PerformanceId)}");
    //
    //     return result.First();
    // }
}