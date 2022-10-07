using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Core.Data;

public interface IUserRepository : IRepository<UserProfile>
{
    public Task<UserProfile?> GetUserByEmail(string email);
}