using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheatersOfTheCity.Api.Controllers.Extensions;
using TheatersOfTheCity.Contracts.Services;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Contracts.v1.Response;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Domain.Filters;

namespace TheatersOfTheCity.Api.Controllers.v1;

public class ParticipantsController : BaseEntitiesController
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ParticipantsController(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    /// <summary>
    /// Get all participants in all performances
    /// </summary>
    /// <returns></returns>
    ///<remarks>new List<DynamicFilter>(){new DynamicFilter(){FieldName = "FirstName", Value = "David", FieldType = 1}}</remarks>

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IEnumerable<ParticipantResponse>),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll( 
        [FromQuery] PaginationFilter paginationFilter,
        [FromQuery] DynamicFilters? dynamicFilters = null,
        [FromQuery] SortFilter? sortQuery = null)
    {
        var participants = await _unitOfWork.ParticipantRepository.PaginateAsync(paginationFilter, sortQuery, dynamicFilters);
        if (participants.data == null)
        {
            return NotFound();
        }

        var mappedParticipants = _mapper.Map<IEnumerable<ParticipantResponse>>(participants.data);
        var response = 
            mappedParticipants.ToPageList(paginationFilter.Page, participants.count, paginationFilter.Size);
        var metadata = PageListHeaderResponseService.PageListHeaderResponse(response);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        return Ok(mappedParticipants.ToApiResponse());
    }
    
    /// <summary>
    /// Get particular participant with role by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Create new participant in particular performance
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Update participant
    /// </summary>
    /// <param name="request"></param>
    /// <param name="id"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Delete participant by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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