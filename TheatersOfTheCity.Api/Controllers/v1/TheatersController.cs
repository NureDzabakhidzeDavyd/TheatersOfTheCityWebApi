using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Api.Controllers.Extensions;
using TheatersOfTheCity.Contracts.Common;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Services;

namespace TheatersOfTheCity.Api.Controllers.v1
{
    [Route("api/[controller]")]
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
        public override async Task<IActionResult> GetAll()
        {
            var theaters = await _unitOfWork.TheaterRepository.GetAllAsync();

            if (!theaters.Any())
            {
                return NotFound();
            }
            
            var response = _mapper.Map<IEnumerable<TheaterResponse>>(theaters);
            return Ok(response);
        }

        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<IActionResult> GetById([FromRoute] int id)
        {
            // var theater = await _unitOfWork.TheaterRepository.GetByIdAsync(id);
            var theater = new Theater();
            theater = null;
            if (theater is null)
            {
                return NotFound(theater.ToApiResponse("Theater doesn't exist"));
            }

            var response = _mapper.Map<TheaterResponse>(theater);
            return Ok(response.ToApiResponse());
        }
        
        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status201Created)]
        public override async Task<IActionResult> Create([FromBody] CreateTheaterRequest request)
        {
            var newTheater = _mapper.Map<Theater>(request);
            var theater = await _unitOfWork.TheaterRepository.CreateAsync(newTheater);
            var response = _mapper.Map<TheaterResponse>(theater);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        
        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status200OK)]
        public override async Task<IActionResult> Update([FromBody] UpdateTheaterRequest request)
        {
            var updateTheater = _mapper.Map<Theater>(request);
            var theater = await _unitOfWork.TheaterRepository.UpdateAsync(updateTheater);
            var response = _mapper.Map<TheaterResponse>(theater);
            return Ok(response);
        }

        [ProducesResponseType(typeof(TheaterResponse), StatusCodes.Status204NoContent)]
        public override async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            await _unitOfWork.TheaterRepository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
