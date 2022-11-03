using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheatersOfTheCity.Api.Controllers.Extensions;

[Route("api/v1/[controller]")]
[Produces("application/json")]
[ResponseCache(CacheProfileName = "120SecondsDuration")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public abstract class BaseEntitiesController : ControllerBase
{
}