using Bogus;
using Microsoft.Extensions.Logging;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Enums;
using TheatersOfTheCity.Core.Services;

namespace TheatersOfTheCity.Data;

public class Seeder : ISeeder
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<Seeder> _logger;

    public Seeder(IUnitOfWork unitOfWork, ILogger<Seeder> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task Seed(bool generateDatabase)
    {
        if (generateDatabase)
        {
          //  await GenerateContacts(20);
            await GenerateTheaters(); 
        }
    }

    /// <summary>
    /// Generate theaters depends on artistic director's contacts count.
    /// </summary>
    public async Task GenerateTheaters()
    {
        _logger.LogInformation("Seeder: Creating theaters");

        var theaters = new List<Theater>();
        var contacts = await _unitOfWork.ContactRepository.GetAllAsync();
        
        var artisticDirectorContacts = contacts
            .Where(x => x.Position == Position.ArtisticDirector);
        foreach (var contact in artisticDirectorContacts)
        {
            var fakeTheater = new Faker<Theater>()
                .RuleFor(x => x.Name, f => f.Commerce.Department())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.StreetAddress())
                .RuleFor(x => x.ArtisticDirector, contact)
                .RuleFor(x => x.ArtisticDirectorId, contact.ContactId)
                .Generate();
            theaters.Add(fakeTheater);
        }

        await _unitOfWork.TheaterRepository.CreateManyAsync(theaters);
        _logger.LogInformation("Seeder: theaters created");
    }

    public async Task GenerateContacts(int count)
    {
        _logger.LogInformation("Seeder: Creating contacts");
        var contacts = new Faker<Contact>()
            .RuleFor(x => x.FirstName, (f => f.Name.FirstName()))
            .RuleFor(x => x.SecondName, f => f.Name.LastName())
            .RuleFor(x => x.Birth, f => f.Date.Past(15, new DateTime(2004, 11, 11)))
            .RuleFor(x => x.Position, f => f.PickRandom<Position>())
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.FirstName, x.SecondName))
            .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
            .Generate(count);

        try
        {
            await _unitOfWork.ContactRepository.CreateManyAsync(contacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        _logger.LogInformation("Seeder: Contacts created");
    }
}