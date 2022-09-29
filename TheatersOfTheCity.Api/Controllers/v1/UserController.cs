using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<ActionResult> UserProfile()
        {
            return BadRequest();
        }

    }
}
