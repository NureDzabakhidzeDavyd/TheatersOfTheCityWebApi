using Microsoft.Extensions.Logging;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Options;

namespace TheatersOfTheCity.Data.Repositories;

public class ContactRepository : BaseRepository<Contact>, IContactRepository
{
    public ContactRepository(RepositoryConfiguration sqlConfiguration, 
        ILogger<BaseRepository<Contact>> logger) : base(sqlConfiguration, logger) {}
}