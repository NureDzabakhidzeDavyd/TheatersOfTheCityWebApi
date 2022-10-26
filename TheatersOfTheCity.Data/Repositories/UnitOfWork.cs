using TheatersOfTheCity.Core.Data;

namespace TheatersOfTheCity.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ITheaterRepository theaterRepository, 
        IPerformanceRepository performanceRepository,
        IContactRepository contactRepository,
        IUserRepository userRepository,
         IProgramRepository programRepository,
        ISceneRepository sceneRepository)
    {
        TheaterRepository = theaterRepository;
        PerformanceRepository = performanceRepository;
        ContactRepository = contactRepository;
        UserRepository = userRepository;
        ProgramRepository = programRepository;
        SceneRepository = sceneRepository;
    }
    
    public ITheaterRepository TheaterRepository { get; set; }
    public IPerformanceRepository PerformanceRepository { get; set; }
    public IContactRepository ContactRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IProgramRepository ProgramRepository { get; set; }
    public ISceneRepository SceneRepository { get; set; }
}