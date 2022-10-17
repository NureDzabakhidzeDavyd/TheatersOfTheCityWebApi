using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Core.Data;

public interface ISceneRepository : IRepository<Scene>
{
    public Task CreateScene(IEnumerable<int> participantsIds, int performanceId);
}