using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Core.Data;

public interface ISceneRepository : IRepository<Scene>
{
    public Task<IEnumerable<Scene>> GetScenesByPerformanceIdAsync(int performanceId);
    public Task DeleteScenesByPerformanceIdAsync(int performanceId);

    // public Task CreateSceneAsync(IEnumerable<Scene> participantsIds, int performanceId);
    // public Task UpdateSceneAsync(IEnumerable<int> participantsIds, int performanceId);
}