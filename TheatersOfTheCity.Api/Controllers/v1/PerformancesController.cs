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
        
        /// <summary>
        /// Get all performances
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<PerformanceResponse>),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var performances = await _unitOfWork.PerformanceRepository.GetAllAsync();
            if (performances == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<IEnumerable<PerformanceResponse>>(performances);
            return Ok(response.ToApiResponse());
        }

        /// <summary>
        /// Get performance by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PerformanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

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
        
        /// <summary>
        /// Get all theaters which shows performance with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/shows")]
        [ProducesResponseType(typeof(Lookup), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetPerfromanceShows([FromRoute] int id)
        {
            var response = await _unitOfWork.PerformanceRepository.GetPerformanceShowsByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            
            return Ok(response.ToApiResponse());
        }

        /// <summary>
        /// Create performance with participants
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(PerformanceResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public  async Task<IActionResult> Create([FromBody]CreatePerformanceRequest request)
        {
            var requestParticipants = _mapper.Map<IEnumerable<Participant>>(request.Participants);
            
            var newPerformance = _mapper.Map<Performance>(request);
            var performance = await _unitOfWork.PerformanceRepository.CreateAsync(newPerformance);
            var response = _mapper.Map<PerformanceResponse>(performance);
            
            if (request.Participants.Any())
            {
                var contacts = await _unitOfWork.ContactRepository.GetManyByIdAsync(requestParticipants.Select(x => x.ContactId), nameof(Contact.ContactId));
                if (contacts.Count() != requestParticipants.Count())
                {
                    NotFound(contacts.ToApiResponse("Not all contacts are exist"));
                }
                
                requestParticipants.ToList().ForEach(x => x.Contact = contacts.First(c => c.ContactId == x.ContactId));
                requestParticipants.ToList().ForEach(x => x.ContactId = x.Contact.ContactId);
                requestParticipants.ToList().ForEach(x => x.Performance = new Lookup(){Id = response.PerformanceId, Name = response.Name});
                requestParticipants.ToList().ForEach(x => x.PerformanceId = response.PerformanceId);


                var participants = await _unitOfWork.ParticipantRepository.CreateManyAsync(requestParticipants);
                response.Participants = _mapper.Map<IEnumerable<ParticipantResponse>>(participants);
            }
            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>
        /// Update performance by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PerformanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdatePerformanceRequest request, [FromRoute] int id)
        {
            var updatePerformanceRequest = await _unitOfWork.PerformanceRepository.GetByIdAsync(id);
            if (updatePerformanceRequest == null)
            {
                return NotFound(updatePerformanceRequest.ToApiResponse("Current performance doesn't exist"));
            }

            var mappedPerformance = _mapper.Map(request, updatePerformanceRequest);
            var performance = await _unitOfWork.PerformanceRepository.UpdateAsync(mappedPerformance);

            var response = _mapper.Map<PerformanceResponse>(performance);
            response.Participants.ToList().ForEach(x => x.Performance.Name = response.Name);
            return Ok(response.ToApiResponse());
        }

        /// <summary>
        /// Delete performance by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteById(int id)
        {
            var performance = await _unitOfWork.PerformanceRepository.GetByIdAsync(id);
            if (performance == null)
            {
                return NotFound();
            }

            await _unitOfWork.PerformanceRepository.DeleteByIdAsync(id);
            
            return NoContent();
        }
        
        /// <summary>
        /// Get performance participants
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/participants")]
        [ProducesResponseType(typeof(ParticipantResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetParticipantsByPerformanceId([FromRoute] int id)
        {
            var participant = await _unitOfWork.ParticipantRepository.GetParticipantsByPerformanceIdAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<ParticipantResponse>(participant);
            return Ok(response.ToApiResponse());
        }
    }
}
