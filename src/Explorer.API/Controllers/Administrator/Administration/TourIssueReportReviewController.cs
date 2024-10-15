using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{

    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/toureIssueReportReview")]
    public class TourIssueReportReviewController : BaseApiController
    {
        private readonly ITourIssueReportService _tourIssueReportService;

        public TourIssueReportReviewController(ITourIssueReportService tourIssueReportService)
        {
            _tourIssueReportService = tourIssueReportService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourIssueReportDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourIssueReportService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

    }
}
