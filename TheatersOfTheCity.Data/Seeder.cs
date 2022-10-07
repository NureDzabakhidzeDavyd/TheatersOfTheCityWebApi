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
            await GenerateContacts(20);
        }
    }

    public async Task GenerateTheaters(int count)
    {
        
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