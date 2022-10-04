using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Contracts.v1.Request;

namespace TheatersOfTheCity.Api.Controllers.Extensions;

[Route("api/v1/[controller]")]
[Produces("application/json")]
[ApiController]
public abstract class BaseEntitiesController : ControllerBase
{
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public abstract Task<IActionResult> GetAll();

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public abstract Task<IActionResult> GetById([FromRoute] int id);

        [HttpPut]
        public abstract Task<IActionResult> Create([FromBody] CreateTheaterRequest request);

        [HttpPost]
        public abstract Task<IActionResult> Update([FromBody] UpdateTheaterRequest request);

        [HttpDelete("{id}")]
        public abstract Task<IActionResult> DeleteById([FromRoute] int id);
}