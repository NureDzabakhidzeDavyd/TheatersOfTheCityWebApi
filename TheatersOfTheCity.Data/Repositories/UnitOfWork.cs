using TheatersOfTheCity.Core.Data;

namespace TheatersOfTheCity.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ITheaterRepository theaterRepository, 
        IPerformanceRepository performanceRepository,
        IContactRepository contactRepository)
    {
        TheaterRepository = theaterRepository;
        PerformanceRepository = performanceRepository;
        ContactRepository = contactRepository;
    }
    
    public ITheaterRepository TheaterRepository { get; set; }
    public IPerformanceRepository PerformanceRepository { get; set; }
    public IContactRepository ContactRepository { get; set; }
}