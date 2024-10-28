using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/person")]

    public class PersonController : BaseApiController
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }
        [HttpGet("{id}")]
        public ActionResult<PersonDto> GetTouristPosition(int id)
        {
            var person = _personService.GetPositionByUserId(id);
            return CreateResponse(person);
        }

        [HttpPut("{userId}/position")]
        public ActionResult<PersonDto> UpdatePersonLocation(int userId, [FromBody] TouristPositionDto positionDto)
        {
            var result = _personService.UpdateTouristPosition(userId, positionDto.Latitude, positionDto.Longitude);
            return CreateResponse(result);
        }
    }
}
