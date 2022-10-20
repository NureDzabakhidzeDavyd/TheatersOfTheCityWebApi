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
    [ProducesResponseType(typeof(ParticipantResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var participants = await _unitOfWork.ParticipantRepository.GetAllAsync();

        if (!participants.Any())
        {
            return NotFound();
        }
            
        var response = _mapper.Map<IEnumerable<ParticipantResponse>>(participants);
        return Ok(response.ToApiResponse());
    }
        
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ParticipantResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var participant = await _unitOfWork.ParticipantRepository.GetByIdAsync(id);
        if (participant is null)
        {
            return NotFound(participant.ToApiResponse("Current contact doesn't exist"));
        }

        var response = _mapper.Map<ContactResponse>(participant);
        return Ok(response.ToApiResponse());
    }
}