using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Contracts.Common;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Services;

namespace TheatersOfTheCity.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IGoogleService _googleService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        
        public AuthController(IGoogleService googleService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _googleService = googleService;
            _unitOfWork = unitOfWork;
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
            return Ok(result.ToApiResponse());
        }

        [HttpPost("user")]
        public async Task<ActionResult> UserByAccessToken(string accessToken)
        {
            var user = await _googleService.GetUserProfile(accessToken);

            var result = _mapper.Map<UserProfileResponse>(user);

            return Ok(result.ToApiResponse());
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessToken(string refreshToken)
        {
            var newAccessToken = await _googleService.RefreshAccessToken(refreshToken);

            var result = _mapper.Map<GoogleAuthCodeResponse>(newAccessToken);

            return Ok(result.ToApiResponse());
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleAuth(string token)
        {
            var googleTokenBody = await _googleService.GetAccessTokenAsync(token);

            var userInfo = await _googleService.GetUserProfile(googleTokenBody.AccessToken);

            var user = await _unitOfWork.UserRepository.GetUserByEmail(userInfo.Email);

            if (user is null)
            {
                userInfo.RefreshToken = googleTokenBody.RefreshToken;
                await _unitOfWork.UserRepository.CreateAsync(userInfo);
            }

            var result = _mapper.Map<UserProfileResponse>(userInfo);
            return Ok(result.ToApiResponse());
        }
    }
}
