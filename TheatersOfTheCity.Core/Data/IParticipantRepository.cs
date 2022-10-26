using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Core.Data;

public interface IParticipantRepository : IRepository<Participant>
{
    public Task<IEnumerable<Participant>> GetParticipantsByPerformanceIdAsync(int performanceId);
    public Task DeleteParticipantsByPerformanceIdAsync(int performanceId);
    public Task<IEnumerable<Participant>> GetParticipantsByContactId(int id);

}