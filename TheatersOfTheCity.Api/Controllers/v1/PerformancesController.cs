using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Api.Controllers.Extensions;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.Data;
using System.Linq;
using TheatersOfTheCity.Contracts.Common;
using TheatersOfTheCity.Contracts.v1.Response;
using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Api.Controllers.v1
{
    
    public class PerformancesController : BaseEntitiesController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PerformancesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var performances = await _unitOfWork.PerformanceRepository.GetAllAsync();
            if (!performances.Any())
            {
                return NotFound();
            }

            var response = _mapper.Map<IEnumerable<PerformanceResponse>>(performances);
            return Ok(response.ToApiResponse());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PerformanceResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var performance = await _unitOfWork.PerformanceRepository.GetByIdAsync(id);
            if (performance == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<PerformanceResponse>(performance);
            return Ok(response.ToApiResponse());
        }

        [HttpPost]
        public  async Task<IActionResult> Create(CreateTheaterRequest request)
        {
            var newPerformance = _mapper.Map<Performance>(request);
            var performance = await _unitOfWork.PerformanceRepository.CreateAsync(newPerformance);
            var response = _mapper.Map<PerformanceResponse>(performance);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTheaterRequest request)
        {
            var updateThPerformance = _mapper.Map<Performance>(request);
            var performance = await _unitOfWork.PerformanceRepository.UpdateAsync(updateThPerformance);
            var response = _mapper.Map<TheaterResponse>(performance);
            return Ok(response.ToApiResponse());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteById(int id)
        {
            var performance = await _unitOfWork.PerformanceRepository.GetByIdAsync(id);
            if (performance == null)
            {
                return NotFound();
            }

            await _unitOfWork.PerformanceRepository.DeleteAsync(performance);
            return NoContent();
        }
    }
}
