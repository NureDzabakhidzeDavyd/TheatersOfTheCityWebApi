using TheatersOfTheCity.Core.Data;

namespace TheatersOfTheCity.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ITheaterRepository theaterRepository, 
        IPerformanceRepository performanceRepository,
        IContactRepository contactRepository,
        IUserRepository userRepository,
         IProgramRepository programRepository,
        IParticipantRepository participantRepository)
    {
        TheaterRepository = theaterRepository;
        PerformanceRepository = performanceRepository;
        ContactRepository = contactRepository;
        UserRepository = userRepository;
        ProgramRepository = programRepository;
        ParticipantRepository = participantRepository;
    }
    
    public ITheaterRepository TheaterRepository { get; set; }
    public IPerformanceRepository PerformanceRepository { get; set; }
    public IContactRepository ContactRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IProgramRepository ProgramRepository { get; set; }
    public IParticipantRepository ParticipantRepository { get; set; }
}