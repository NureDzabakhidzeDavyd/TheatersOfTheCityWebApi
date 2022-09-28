using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.External;
using TheatersOfTheCity.Core.Services;

namespace TheatersOfTheCity.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IGoogleService _googleService;
        private readonly IMapper _mapper;
        
        public AuthController(IGoogleService googleService, IMapper mapper)
        {
            _googleService = googleService;
            _mapper = mapper;
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> GoogleAuthAccessToken()
        {
            var authCodeResponse = await SendGoogleAuthRequest();
            var result = JsonConvert.DeserializeObject<GoogleAuthBody>(authCodeResponse.ToString() ?? throw new InvalidOperationException());

            return null;
        }

        [HttpPost("code")]
        public async Task<IActionResult> SendGoogleAuthRequest()
        {
            string url = await _googleService.GoogleAuthUrlRequest();
            return Redirect(url);
        }
    }
}
