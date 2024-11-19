using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/user/personal-dairy")]
    public class PersonalDairyController : BaseApiController
    {
        private readonly IPersonalDairyService _personalDairyService;

        public PersonalDairyController(IPersonalDairyService personalDairyService)
        {
            _personalDairyService = personalDairyService;
        }

        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<PersonalDairyDto>> GetAllForUser(long userId)
        {
            var result = _personalDairyService.GetAllForUser(userId);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult<PersonalDairyDto> Create([FromBody] PersonalDairyDto dto)
        {
            var result = _personalDairyService.Create(dto);
            return CreateResponse(result);
        }

        [HttpPut("{id}")]
        public ActionResult<PersonalDairyDto> Update(long id, [FromBody] PersonalDairyDto dto)
        {
            var result = _personalDairyService.Update(id, dto);
            return CreateResponse(result);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            var result = _personalDairyService.Delete(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Personal dairy deleted successfully.");
        }
    }
}
