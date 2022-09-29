using AutoMapper;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.External;

namespace TheatersOfTheCity.Api.Mapper;

public class RequestToDomainProfile : Profile
{
    public RequestToDomainProfile()
    {
        CreateMap<GoogleTokenBody, GoogleAuthCodeResponse>();
    }
}