using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class TheaterRepository : BaseRepository<Theater>, ITheaterRepository
{
    public TheaterRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration) {}
}