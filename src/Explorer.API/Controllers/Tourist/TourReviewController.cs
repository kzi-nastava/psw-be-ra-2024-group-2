using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tour/reviews")]
    public class TourReviewController : BaseApiController
    {
        private readonly ITourReviewService _tourReviewService;

        public TourReviewController(ITourReviewService tourReviewService)
        {
            _tourReviewService = tourReviewService;
        }

        [HttpPost]
        public ActionResult<TourReviewDto> Create([FromBody] TourReviewDto review)
        {
            var result = _tourReviewService.Create(review);
            return CreateResponse(result);
        }

        [HttpGet("paged/{tourId}")]
        public ActionResult<PagedResult<TourReviewDto>> GetPagedByTourId(int tourId)
        {
            var result = _tourReviewService.GetPagedByTourId(tourId, 1, int.MaxValue);
            return CreateResponse(result);
        }
    }
}
