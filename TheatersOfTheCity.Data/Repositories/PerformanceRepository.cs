using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class PerformanceRepository : BaseRepository<Performance>, IPerformanceRepository
{
    public PerformanceRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration) {}
}