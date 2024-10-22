﻿using System.Data.SqlTypes;
using Bogus;
using Microsoft.Extensions.Logging;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
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
    
    public async Task Seed()
    {
        var contacts = await _unitOfWork.ContactRepository.GetAllAsync();
        if (!contacts.Any())
        {
             await GenerateContacts(50);
              await GenerateTheaters(10); 
              await GeneratePerformances(20);
              await GeneratePrograms();
              await GenerateParticipants(20);
        }
    }

    private async Task GenerateParticipants(int count)
    {
        var roles = new[]
        {
            "Artistic director",
            "Theater manager",
            "Music Director",
            "Technical director",
            "Costume director",
            "Marketing director",
            "Director of public relations",
            "Director of audience services",
            "Director of development",
            "Director of special events",
            "Dramaturge",
            "Literary manager",
            "Company manager",
            "House manager",
            "Usher",
            "Ticketing agent",
            "Crew chief",
            "Janitor",
            "Dresser",
            "Stage crew",
            "Fly crew",
            "Light board operator",
            "Spotlight operator",
            "Grips",
            "Call boy",
            "Wardrobe Crew"
        };

        var actors = await _unitOfWork.ContactRepository.GetAllAsync();
        var performances = (await _unitOfWork.PerformanceRepository.GetAllAsync()).ToArray();

        if (!performances.Any() || !actors.Any())
        {
            _logger.LogError("Performances or contacts don't exist");
            throw new SqlNullValueException();
        }

        _logger.LogInformation("Seeder: Creating participants");
        var result = new List<Participant>();
        foreach (var performance in performances)
        {
            var participants = new Faker<Participant>()
            .RuleFor(x => x.PerformanceId, performance.PerformanceId)
            .RuleFor(x => x.ContactId, f => f.PickRandom(actors).ContactId)
            .RuleFor(x => x.Role, f => f.PickRandom(roles))
            .Generate(3);
            result.AddRange(participants);
        }

        await _unitOfWork.ParticipantRepository.CreateManyAsync(result);
        _logger.LogInformation("Seeder: Participants created");
    }

    /// <summary>
    /// Generate theaters depends on artistic director's contacts count.
    /// </summary>
    public async Task GenerateTheaters(int count)
    {
        _logger.LogInformation("Seeder: Creating theaters");
        var theaters = new List<Theater>();
        
        var contacts = (await _unitOfWork.ContactRepository.GetAllAsync()).Take(count);
        foreach (var contact in contacts)
        {
            var theater = new Faker<Theater>()
                .RuleFor(x => x.Name, f => f.Commerce.Department())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.Address, f => f.Address.StreetAddress())
                .RuleFor(x => x.DirectorId, contact.ContactId)
                .Generate();
            theaters.Add(theater);
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
            .RuleFor(x => x.Birth, f => f.Person.DateOfBirth.Date)
            .RuleFor(x => x.Email, (f, x) => f.Internet.Email(x.FirstName, x.SecondName))
            .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
            .Generate(count);
        
            await _unitOfWork.ContactRepository.CreateManyAsync(contacts);
            _logger.LogInformation("Seeder: Contacts created");
    }

    public async Task GeneratePrograms()
    {
        var programs = new List<Program>();
        var performances = await _unitOfWork.PerformanceRepository.GetAllAsync();
        var theaters = await _unitOfWork.TheaterRepository.GetAllAsync();
        var faker = new Faker<Program>();
        
        _logger.LogInformation("Seeder: Creating programs");
        foreach (var performance in performances)
        {
             var newProgram = faker.RuleFor(x => x.TheaterId, f => f.PickRandom(theaters).TheaterId)
                .RuleFor(x => x.PerformanceId, performance.PerformanceId)
                .Generate();
             programs.Add(newProgram);
        }
    
        await _unitOfWork.ProgramRepository.CreateManyAsync(programs);
        _logger.LogInformation("Seeder: Programs created");
    }

    public async Task GeneratePerformances(int count)
    {
        var genres = new[]
        {
            "Action",
            "Adventure",
            "Animated",
            "Biography",
            "Comedy",
            "Crime",
            "Dance",
            "Disaster",
            "Documentary",
            "Drama",
            "Erotic",
            "Family",
            "Fantasy",
            "Found Footage",
            "Historical",
            "Horror",
            "Independent",
            "Legal",
            "Live Action",
            "Martial Arts",
            "Musical",
            "Mystery",
            "Noir",
            "Performance",
            "Political",
            "Romance",
            "Satire",
            "Science Fiction",
            "Short",
            "Silent",
            "Slasher",
            "Sports",
            "Spy",
            "Superhero",
            "Supernatural",
            "Suspense",
            "Teen",
            "Thriller",
            "War",
            "Western"
        };
        
        _logger.LogInformation("Seeder: Creating performances");
        
        var performances = new Faker<Performance>()
            .RuleFor(x => x.Name, f => f.Company.CompanyName())
            .RuleFor(x => x.Genre, f => f.PickRandom(genres))
            .RuleFor(x => x.Duration, f => f.Date.BetweenTimeOnly(new TimeOnly(1, 0), new TimeOnly(4, 0)).ToTimeSpan())
            .RuleFor(x => x.Language, f => f.PickRandom(new[] { "en", "ua" }))
            .Generate(count);
        await _unitOfWork.PerformanceRepository.CreateManyAsync(performances);
        
        _logger.LogInformation("Seeder: Performances created");
    }
}