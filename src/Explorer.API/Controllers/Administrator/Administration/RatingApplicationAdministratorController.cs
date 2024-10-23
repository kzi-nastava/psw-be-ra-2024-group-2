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

        public RatingApplicationAdministratorController(IRatingApplicationService ratingApplicationService)
        {
            _ratingApplicationService = ratingApplicationService;
        }


        [HttpGet]
        public ActionResult<PagedResult<RatingApplicationDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _ratingApplicationService.GetPaged(page, pageSize);
            return CreateResponse(result);
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
