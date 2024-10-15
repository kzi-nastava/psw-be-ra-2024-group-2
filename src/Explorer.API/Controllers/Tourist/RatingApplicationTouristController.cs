using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/ratingApplication")]
    public class RatingApplicationTouristController : BaseApiController
    {

        private readonly IRatingApplicationService _ratingApplicationService;

        public RatingApplicationTouristController(IRatingApplicationService ratingApplicationService)
        {
            _ratingApplicationService = ratingApplicationService;
        }

        [HttpPost]
        public ActionResult<RatingApplicationDto> Create([FromBody] RatingApplicationDto applicationRatingDto)
        {
            var result = _ratingApplicationService.Create(applicationRatingDto.UserId, applicationRatingDto);
            return CreateResponse(result);
        }



        [HttpGet]
        public ActionResult<RatingApplicationDto> Get()
        {
            var rating = _ratingApplicationService.Get(User.PersonId());
            return CreateResponse(rating);
        }
    }
}
