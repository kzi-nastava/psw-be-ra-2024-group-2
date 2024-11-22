using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/ratingApplication")]
    public class RatingApplicationAuthorController : BaseApiController
    {
        private readonly IRatingApplicationService _ratingApplicationService;
        private readonly IPersonService _personService;

        public RatingApplicationAuthorController(IRatingApplicationService ratingApplicationService, IPersonService personService)
        {
            _ratingApplicationService = ratingApplicationService;
            _personService = personService;
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

            if (rating.IsFailed || rating.Value == null)
            {
                return BadRequest(rating.Errors);
            }

            var user = _personService.GetAccountImage(rating.Value.UserId);

            RatingWithUserDto ratingWithUserDto = new RatingWithUserDto()
            {
                RatingApplication = rating.Value,
                Account = user.Value
            };

            var result = Result.Ok(ratingWithUserDto);
            return CreateResponse(result);
        }
    }
}
