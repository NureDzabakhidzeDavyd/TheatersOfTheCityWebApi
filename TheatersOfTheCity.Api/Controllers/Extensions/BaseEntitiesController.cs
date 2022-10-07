using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Contracts.v1.Request;

namespace TheatersOfTheCity.Api.Controllers.Extensions;

[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public abstract class BaseEntitiesController : ControllerBase
{
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public abstract Task<IActionResult> GetAll();

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public abstract Task<IActionResult> GetById([FromRoute] int id);

        [HttpDelete("{id}")]
        public abstract Task<IActionResult> DeleteById([FromRoute] int id);
}