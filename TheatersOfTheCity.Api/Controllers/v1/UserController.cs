using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Core.Services;

namespace TheatersOfTheCity.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGoogleService _googleService;
        private readonly IMapper _mapper;
        
        public UserController(IGoogleService googleService, IMapper mapper)
        {
            _googleService = googleService;
            _mapper = mapper;
        }    
    }
}
