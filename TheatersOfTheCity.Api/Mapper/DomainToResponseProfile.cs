using AutoMapper;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Api.Mapper;

public class DomainToResponseProfile : Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<UserProfile, UserProfileResponse>();
        
        CreateMap<Theater, TheaterResponse>();
    }
}