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

        [HttpPost]
        [ProducesResponseType(typeof(PerformanceResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public  async Task<IActionResult> Create(CreatePerformanceRequest request)
        {
            var participants = _mapper.Map<IEnumerable<Participant>>(request.Participants);
            var contactsIds = request.Participants.Select(x => x.ContactId);
            
            var newPerformance = _mapper.Map<Performance>(request);
            var performance = await _unitOfWork.PerformanceRepository.CreateAsync(newPerformance);
            var response = _mapper.Map<PerformanceResponse>(performance);

            if (request.Participants.Any())
            {
                var contacts =
                    await _unitOfWork.ContactRepository.GetManyByIdAsync(contactsIds, nameof(Contact.ContactId));
                if (contacts.Count() != contactsIds.Count())
                {
                    NotFound(contactsIds.ToApiResponse("Not all contacts are exist"));
                }
                    // TODO: Make less query request to db. Multiply insert in multiply tables
                var newParticipants = await _unitOfWork.ParticipantRepository.CreateManyAsync(participants);
                await _unitOfWork.SceneRepository.CreateSceneAsync(newParticipants.Select(x => x.ContactId),
                    performance.PerformanceId);
                newParticipants.ToList().ForEach(x => x.Contact = contacts.First(c => c.ContactId == x.ContactId));
                response.Participants = _mapper.Map<IEnumerable<ParticipantResponse>>(newParticipants);
            }
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdatePerformanceRequest request, [FromRoute] int id)
        {
            var updateThPerformance = await _unitOfWork.PerformanceRepository.GetByIdAsync(id);
            if (updateThPerformance == null)
            {
                return NotFound(updateThPerformance.ToApiResponse("Current performance doesn't exist"));
            }

            var mappedPerformance = _mapper.Map(request, updateThPerformance);
            var performance = await _unitOfWork.PerformanceRepository.UpdateAsync(mappedPerformance);
            var participants = updateThPerformance.Participants;

            var response = _mapper.Map<PerformanceResponse>(performance);
            response.Participants = _mapper.Map<IEnumerable<ParticipantResponse>>(participants);
            return Ok(response.ToApiResponse());
            // TODO: Update Participants or no?
        }

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

            await _unitOfWork.PerformanceRepository.DeleteAsync(performance);
            return NoContent();
        }
    }
}
