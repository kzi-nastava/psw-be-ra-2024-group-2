using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{

    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/tourIssueReportReview")]
    public class TourIssueReportReviewController : BaseApiController
    {
        private readonly ITourIssueReportService _tourIssueReportService;
        private readonly ITourService _tourService;

        public TourIssueReportReviewController(ITourIssueReportService tourIssueReportService, ITourService tourService)
        {
            _tourIssueReportService = tourIssueReportService;
            _tourService = tourService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourIssueReportDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourIssueReportService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }


        [HttpGet("{tourId}")]
        public ActionResult<TourDto> GetById(long tourId)
        {
            var tour = _tourService.GetById(tourId);
            return CreateResponse(tour);
        }

    }
}
