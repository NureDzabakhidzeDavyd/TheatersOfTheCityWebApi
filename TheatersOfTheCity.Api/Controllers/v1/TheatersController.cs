using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Api.Controllers.Extensions;
using TheatersOfTheCity.Contracts.Common;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Contracts.v1.Response;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TheatersController : BaseEntitiesController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        
        public TheatersController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(IEnumerable<TheaterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var theaters = await _unitOfWork.TheaterRepository.GetAllAsync();

            if (!theaters.Any())
            {
                return NotFound();
            }
            
            var response = _mapper.Map<IEnumerable<TheaterResponse>>(theaters);
            return Ok(response.ToApiResponse());
        }

        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        { 
            var theater = await _unitOfWork.TheaterRepository.GetByIdAsync(id);
            if (theater == null)
            {
                return NotFound(theater.ToApiResponse("Theater doesn't exist"));
            }

            var response = _mapper.Map<TheaterResponse>(theater);
            return Ok(response.ToApiResponse());
        }
        
        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        { 
            var theater = await _unitOfWork.TheaterRepository.GetTheaterByName(name);
            if (theater == null)
            {
                return NotFound(theater.ToApiResponse("Theater doesn't exist"));
            }

            var response = _mapper.Map<TheaterResponse>(theater);
            return Ok(response.ToApiResponse());
        }
        
        [ProducesResponseType(typeof(IEnumerable<TheaterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/programs")]
        public async Task<IActionResult> GetTheaterPrograms([FromRoute] int id)
        {
            var theater = await _unitOfWork.TheaterRepository.GetByIdAsync(id);
            if (theater == null)
            {
                return NotFound(theater.ToApiResponse("Theater was not found"));
            }
            
            var performances = await _unitOfWork.PerformanceRepository.GetTheaterProgramsAsync(id);

            if (!performances.Any())
            {
                return NotFound(theater.ToApiResponse("Theater doesn't have any performances"));
            }

            var response = _mapper.Map<IEnumerable<LookupResponse>>(performances);
            return Ok(response.ToApiResponse());
        }
        
        [HttpPost]
        [ProducesResponseType( StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateTheaterRequest request)
        {
            var newTheater = _mapper.Map<Theater>(request);
            
            var director = await _unitOfWork.ContactRepository.GetByIdAsync(request.DirectorId);
            if (director == null)
            {
                return NotFound(director.ToApiResponse($"Director with {request.DirectorId} doesn't exist"));
            }
            
            var theater = await _unitOfWork.TheaterRepository.CreateAsync(newTheater);
            
            var response = _mapper.Map<TheaterResponse>(theater);
            response.Director = _mapper.Map<ContactResponse>(director);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateTheaterRequest request, [FromRoute] int id)
        {
            var updateTheater = await _unitOfWork.TheaterRepository.GetByIdAsync(id);
            if (updateTheater == null)
            {
                return NotFound(updateTheater.ToApiResponse($"Theater with {id} id doesn't exist"));
            }
            
            var director = await _unitOfWork.ContactRepository.GetByIdAsync(request.DirectorId);
            if (director == null)
            {
                return NotFound(director.ToApiResponse($"Director with {request.DirectorId} doesn't exist"));
            }

            var theater = await _unitOfWork.TheaterRepository.UpdateAsync(_mapper.Map(request, updateTheater));
            
            var response = _mapper.Map<TheaterResponse>(theater);
            response.Director = _mapper.Map<ContactResponse>(director);
            
            return Ok(response.ToApiResponse());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            await _unitOfWork.TheaterRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
