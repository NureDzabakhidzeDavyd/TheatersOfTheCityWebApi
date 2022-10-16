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
        
        [HttpPost]
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
            response.Director = theater.Director;
            return StatusCode(StatusCodes.Status201Created, response);
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateTheaterRequest request)
        {
            var updateTheater = _mapper.Map<Theater>(request);
            var theater = await _unitOfWork.TheaterRepository.UpdateAsync(updateTheater);
            
            var director = await _unitOfWork.ContactRepository.GetByIdAsync(request.DirectorId);
            if (director == null)
            {
                return NotFound(director.ToApiResponse($"Director with {request.DirectorId} doesn't exist"));
            }

            await _unitOfWork.TheaterRepository.UpdateAsync(theater);
            
            var response = _mapper.Map<TheaterResponse>(theater);
            response.Director = director;
            
            return Ok(response.ToApiResponse());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            await _unitOfWork.TheaterRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
