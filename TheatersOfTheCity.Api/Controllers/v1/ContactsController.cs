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
    public class ContactsController : BaseEntitiesController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ContactsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _unitOfWork.ContactRepository.GetAllAsync();

            if (!contacts.Any())
            {
                return NotFound();
            }
            
            var response = _mapper.Map<IEnumerable<Contact>>(contacts);
            return Ok(response.ToApiResponse());
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
        {
            var contact = _mapper.Map<Contact>(request);
            var newContact = await _unitOfWork.ContactRepository.CreateAsync(contact);
            var response = _mapper.Map<ContactResponse>(newContact);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateContactRequest request)
        {
            var updateContact = _mapper.Map<Contact>(request);
            var contact = await _unitOfWork.ContactRepository.UpdateAsync(updateContact);
            var response = _mapper.Map<ContactResponse>(contact);
            return Ok(response.ToApiResponse());
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _unitOfWork.ContactRepository.GetByIdAsync(id);
            if (contact is null)
            {
                return NotFound();
            }

            var response = _mapper.Map<Contact>(contact);
            return Ok(response.ToApiResponse());
        }

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
    }
}
