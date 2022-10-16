using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class SceneRepository : BaseRepository<Scene>, ISceneRepository
{
    public SceneRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
    }
}