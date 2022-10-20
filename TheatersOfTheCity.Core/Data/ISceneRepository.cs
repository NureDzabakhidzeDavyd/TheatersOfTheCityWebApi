using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Core.Data;

public interface ISceneRepository : IRepository<Scene>
{
    public Task CreateSceneAsync(IEnumerable<int> participantsIds, int performanceId);
    public Task UpdateSceneAsync(IEnumerable<int> participantsIds, int performanceId);
}