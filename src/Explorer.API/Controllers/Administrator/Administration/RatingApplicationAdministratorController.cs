using Explorer.Stakeholders.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administrator/ratingApplication")]
    public class RatingApplicationAdministratorController :BaseApiController
    {

        private readonly IRatingApplicationService _ratingApplicationService;
        private readonly IPersonService _personService;

        public RatingApplicationAdministratorController(IRatingApplicationService ratingApplicationService, IPersonService personService)
        {
            _ratingApplicationService = ratingApplicationService;
            _personService = personService;
        }


        [HttpGet]
        public ActionResult<PagedResult<RatingWithUserDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _ratingApplicationService.GetPaged(page, pageSize);

            if (!result.IsSuccess)
            {
                return CreateResponse(result);
            }

            var ratingApplications = result.Value.Results;

            var userIds = ratingApplications.Select(r => r.UserId).Distinct().ToList();

            var accountsResult = _personService.GetManyAccountImage(userIds);

            if (!accountsResult.IsSuccess)
            {
                return BadRequest(new { errors = accountsResult.Errors });
            }

            var accountImages = accountsResult.Value;

            var ratingWithUserDtos = ratingApplications.Select(rating =>
            {
                var accountImage = accountImages.FirstOrDefault(ai => ai.Id == rating.UserId);
                return new RatingWithUserDto
                {
                    RatingApplication = rating,
                    Account = accountImage
                };
            }).ToList();

            var pagedResult = new PagedResult<RatingWithUserDto>(ratingWithUserDtos, result.Value.TotalCount);

            return Ok(pagedResult);
        }

        [HttpPost]
        public ActionResult<RatingApplicationDto> Create([FromBody] RatingApplicationDto applicationRatingDto)
        {
            applicationRatingDto.UserId = User.PersonId();
            var result = _ratingApplicationService.Create(applicationRatingDto.UserId, applicationRatingDto);
            return CreateResponse(result);
        }



    }
}
