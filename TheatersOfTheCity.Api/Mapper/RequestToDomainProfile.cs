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
        
        CreateMap<CreateContactRequest, Contact>();
        CreateMap<UpdateContactRequest, Contact>();

        CreateMap<CreateTheaterRequest, Theater>();
        CreateMap<UpdateTheaterRequest, Theater>();
        
        CreateMap<CreatePerformanceRequest, Performance>();
        CreateMap<UpdatePerformanceRequest, Performance>();
        CreateMap<CreatePerformanceParticipantRequest, Participant>();

        CreateMap<UpdateParticipantRequest, Participant>();
        CreateMap<CreateParticipantRequest, Participant>();

    }
}