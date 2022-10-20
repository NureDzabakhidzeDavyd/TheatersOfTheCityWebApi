using TheatersOfTheCity.Core.Domain;


namespace TheatersOfTheCity.Core.Data;

public interface IPerformanceRepository : IRepository<Performance>
{
    public Task<IEnumerable<Lookup>> GetTheaterProgramsAsync(int Id);
}