using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tourIssueReport")]
    public class TourIssueReportController : BaseApiController
    {
        private readonly ITourIssueReportService _tourIssueReportService;
        private readonly ITourService _tourService;

        public TourIssueReportController(ITourIssueReportService tourIssueReportService, ITourService tourService)
        {
            _tourIssueReportService = tourIssueReportService;
            _tourService = tourService;
        }

        [HttpPost]
        public ActionResult<TourIssueReportDto> Create([FromBody] TourIssueReportDto report)
        {
            var result = _tourIssueReportService.Create(report.UserId, report.TourId, report);
            return CreateResponse(result);
        }

        [HttpGet]
        public ActionResult<PagedResult<TourDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
    }
}
