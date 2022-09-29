using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
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
        
        /// <summary>
        /// Get refresh and access token by code
        /// </summary>
        /// <param name="authCode">The authorization code returned from the initial request</param>
        /// <returns>The object with refresh and access token + remaining token lifetime</returns>
        [HttpPost("code")]
        public async Task<IActionResult> RefreshAndAccessToken(string authCode)
        {
            var token = await _googleService.GetAccessTokenAsync(authCode);

            var result = _mapper.Map<GoogleAuthCodeResponse>(token);
            return Ok(result);
        }

        [HttpPost("user")]
        public async Task<ActionResult> UserByAccessToken(string accessToken)
        {
            var user = await _googleService.GetUserProfile(accessToken);

            var response = _mapper.Map<UserProfileResponse>(user);

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessToken(string refreshToken)
        {
            var newAccessToken = await _googleService.RefreshAccessToken(refreshToken);

            var result = _mapper.Map<GoogleAuthCodeResponse>(newAccessToken);

            return Ok(result);
        }
    }
}
