using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.UseCases.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Tours.API.Public.Administration;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tour/preferences")]
    public class TourPreferenceController : BaseApiController
    {
        private readonly ITourPreferenceService _tourPreferenceService;
        public TourPreferenceController(ITourPreferenceService tourPreferenceService)
        {
            _tourPreferenceService = tourPreferenceService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourPreferenceDto>> GetByTouristId()
        {
            var allTourPreferences = _tourPreferenceService.GetByTouristId(User.PersonId());
            return CreateResponse(allTourPreferences.ToResult());
        }

        [HttpPost]
        public ActionResult<TourPreferenceDto> Create([FromBody] TourPreferenceDto tourPreference)
        {
            var result = _tourPreferenceService.Create(tourPreference);
            return CreateResponse(result);
        }

    }
}
