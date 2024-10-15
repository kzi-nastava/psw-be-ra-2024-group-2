using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/toureIssueReport")]
    public class TourIssueReportController : BaseApiController
    {
        private readonly ITourIssueReportService _tourIssueReportService;

        public TourIssueReportController(ITourIssueReportService tourIssueReportService)
        {
            _tourIssueReportService = tourIssueReportService;
        }

        [HttpPost]
        public ActionResult<TourIssueReportDto> Create([FromBody] TourIssueReportDto report)
        {
            var result = _tourIssueReportService.Create(report.UserId, report.TourId, report);
            return CreateResponse(result);
        }
    }
}
