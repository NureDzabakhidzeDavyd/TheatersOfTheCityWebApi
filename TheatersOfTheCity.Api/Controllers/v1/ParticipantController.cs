using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Api.Controllers.Extensions;
using TheatersOfTheCity.Contracts.Common;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Contracts.v1.Response;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;

namespace TheatersOfTheCity.Api.Controllers.v1;

public class ParticipantController : BaseEntitiesController
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ParticipantController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IEnumerable<ParticipantResponse>),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var participants = await _unitOfWork.ParticipantRepository.GetAllAsync();
        if (participants == null)
        {
            return NotFound();
        }

        var response = _mapper.Map<IEnumerable<ParticipantResponse>>(participants);
        return Ok(response.ToApiResponse());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ParticipantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var participant = await _unitOfWork.ParticipantRepository.GetByIdAsync(id);
        if (participant == null)
        {
            return NotFound();
        }

        var response = _mapper.Map<ParticipantResponse>(participant);
        return Ok(response.ToApiResponse());
    }

    [HttpPost]
    [ProducesResponseType(typeof(ParticipantResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public  async Task<IActionResult> Create([FromBody] CreateParticipantRequest request)
    {
        var requestParticipant = _mapper.Map<Participant>(request);
        var participant = await _unitOfWork.ParticipantRepository.CreateAsync(requestParticipant);

        var contact = await _unitOfWork.ContactRepository.GetByIdAsync(request.ContactId);
        if (contact == null)
        {
            return NotFound(contact.ToApiResponse("Contact doesn't exist"));
        }

        var performance = await _unitOfWork.PerformanceRepository.GetByIdAsync(request.PerformanceId);
        if (performance == null)
        {
            return NotFound(performance.ToApiResponse("Performance doesn't exist"));
        }
        
        
        var response = _mapper.Map<ParticipantResponse>(participant);
        response.Contact = contact;
        response.Performance = new Lookup() { Id = performance.PerformanceId, Name = performance.Name };

        return StatusCode(StatusCodes.Status201Created, response);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ParticipantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateParticipantRequest request, [FromRoute] int id)
    {
        // TODO: Change participant update method to make less query to database
        var checkParticipant = await _unitOfWork.ParticipantRepository.GetByIdAsync(id);
        if (checkParticipant == null)
        {
            return NotFound(checkParticipant.ToApiResponse("This participant doesn't exist"));
        }
        
        var mappedParticipant = _mapper.Map<Participant>(request);
        mappedParticipant.ParticipantId = id;
        await _unitOfWork.ParticipantRepository.UpdateAsync(mappedParticipant);
        
        var participant = await _unitOfWork.ParticipantRepository.GetByIdAsync(id);

        var response = _mapper.Map<ParticipantResponse>(participant);
        return Ok(response.ToApiResponse());
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteById(int id)
    {
        var participant = await _unitOfWork.ParticipantRepository.GetByIdAsync(id);
        if (participant == null)
        {
            return NotFound();
        }

        await _unitOfWork.ParticipantRepository.DeleteByIdAsync(id);
            
        return NoContent();
    }
}