using AutoMapper;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Contracts.v1.Response;
using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Api.Mapper;

public class DomainToResponseProfile : Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<UserProfile, UserProfileResponse>();
        
        CreateMap<Theater, TheaterResponse>();
        CreateMap<Performance, PerformanceResponse>();
        CreateMap<Contact, ContactResponse>();
    }
}