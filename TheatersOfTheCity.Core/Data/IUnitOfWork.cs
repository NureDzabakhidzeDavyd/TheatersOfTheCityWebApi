namespace TheatersOfTheCity.Core.Data;

public interface IUnitOfWork
{
    public ITheaterRepository TheaterRepository { get; set; }
    public IPerformanceRepository PerformanceRepository { get; set; } 
    public IContactRepository ContactRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IProgramRepository ProgramRepository { get; set; }
    public IParticipantRepository ParticipantRepository { get; set; }
}