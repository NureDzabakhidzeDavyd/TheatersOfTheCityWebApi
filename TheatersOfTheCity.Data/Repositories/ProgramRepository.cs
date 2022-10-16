using Microsoft.Extensions.Logging;
using SqlKata;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class ProgramRepository : BaseRepository<TheatersOfTheCity.Core.Domain.Program>, IProgramRepository
{
    public ProgramRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
    }
}