using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatersOfTheCity.Api.Controllers.Extensions;
using TheatersOfTheCity.Contracts.Common;
using TheatersOfTheCity.Contracts.v1.Request;
using TheatersOfTheCity.Contracts.v1.Response;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Domain.Filters;

namespace TheatersOfTheCity.Api.Controllers.v1
{
    public class ContactsController : BaseEntitiesController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ContactsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        /// <remarks>new List<DynamicFilter>(){new DynamicFilter(){FieldName = "FirstName", Value = "David", FieldType = 1}}</remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationFilter paginationQuery,
            [FromQuery] DynamicFilters? dynamicFilters = null,
            [FromQuery] SortFilter? sortQuery = null)
        {
            var contacts = await _unitOfWork.ContactRepository.PaginateAsync(paginationQuery, sortQuery, dynamicFilters);

            if (!contacts.Any())
            {
                return NotFound();
            }
            
            var response = _mapper.Map<IEnumerable<ContactResponse>>(contacts);
            return Ok(response.ToApiResponse());
        }
        
        /// <summary>
        /// Create new contact
        /// </summary>
        /// <param name="request">New contact request</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
        {
            var contact = _mapper.Map<Contact>(request);
            var newContact = await _unitOfWork.ContactRepository.CreateAsync(contact);
            var response = _mapper.Map<ContactResponse>(newContact);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        
        /// <summary>
        /// Update contact by id
        /// </summary>
        /// <param name="request">New contact' changes</param>
        /// <param name="id">Contact current id</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
        [ProducesResponseType( StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UpdateContactRequest request, [FromRoute] int id)
        {
            var contact = await _unitOfWork.ContactRepository.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound(contact.ToApiResponse("Current contact doesn't exist"));
            }

            var updateContact = _mapper.Map(request, contact);
            var result = await _unitOfWork.ContactRepository.UpdateAsync(updateContact);
            var response = _mapper.Map<ContactResponse>(result);
            return Ok(response.ToApiResponse());
        }
        
        /// <summary>
        /// Get contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _unitOfWork.ContactRepository.GetByIdAsync(id);
            if (contact is null)
            {
                return NotFound(contact.ToApiResponse("Current contact doesn't exist"));
            }

            var response = _mapper.Map<ContactResponse>(contact);
            return Ok(response.ToApiResponse());
        }

        /// <summary>
        /// Delete contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(int id)
        {
            var contact = await _unitOfWork.ContactRepository.GetByIdAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            await _unitOfWork.ContactRepository.DeleteAsync(contact);
            return NoContent();
        }
        
        /// <summary>
        /// Get all roles of contact in performances by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/{id}/participants")]
        [ProducesResponseType(typeof(IEnumerable<Participant>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetParticipantsByContactId([FromRoute] int id)
        {
            var participants = await _unitOfWork.ParticipantRepository.GetParticipantsByContactId(id);
            if (!participants.Any())
            {
                return NotFound();
            }

            var response = _mapper.Map<IEnumerable<Participant>>(participants);
            return Ok(response.ToApiResponse());
        }
    }
}
