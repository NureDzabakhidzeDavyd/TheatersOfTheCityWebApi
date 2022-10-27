using TheatersOfTheCity.Core.Domain;


namespace TheatersOfTheCity.Core.Data;

public interface IPerformanceRepository : IRepository<Performance>
{
    public Task<IEnumerable<Performance>> GetTheaterProgramsAsync(int Id);
    public Task<IEnumerable<Lookup>> GetPerformanceShowsByIdAsync(int id);
}