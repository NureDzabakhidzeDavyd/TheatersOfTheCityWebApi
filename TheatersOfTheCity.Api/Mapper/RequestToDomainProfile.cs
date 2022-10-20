using AutoMapper;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.External;

namespace TheatersOfTheCity.Api.Mapper;

public class RequestToDomainProfile : Profile
{
    public RequestToDomainProfile()
    {
        CreateMap<GoogleTokenBody, GoogleAuthCodeResponse>();
        CreateMap<CreateTheaterRequest, Theater>();
        
        CreateMap<CreateContactRequest, Contact>();
        CreateMap<UpdateContactRequest, Contact>();
        
        CreateMap<CreatePerformanceRequest, Performance>();
        CreateMap<UpdatePerformanceRequest, Performance>();

        CreateMap<CreateParticipantRequest, Participant>();
    }
}