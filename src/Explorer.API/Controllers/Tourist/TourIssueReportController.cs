using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
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
        private readonly ITourIssueCommentService _tourCommentService;

        public TourIssueReportController(ITourIssueReportService tourIssueReportService, ITourService tourService, ITourIssueCommentService tourCommentService)
        {
            _tourIssueReportService = tourIssueReportService;
            _tourService = tourService;
            _tourCommentService = tourCommentService;
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

        [HttpPut("resolvedReport")]
        public ActionResult<TourIssueReportDto> MarkAsDone([FromBody] TourIssueReportDto report)
        {
            var result = _tourIssueReportService.MarkAsDone(report);
            return CreateResponse(result);
        }

        [HttpPut("alertAdmin")]
        public ActionResult<TourIssueReportDto> AlertNotDone([FromBody] TourIssueReportDto report)
        {
            var result = _tourIssueReportService.AlertNotDone(report);
            return CreateResponse(result);
        }

        [HttpPost("comment")]
        public ActionResult<TourIssueCommentDto> CreateComment([FromBody] TourIssueCommentDto comment)
        {
            var result = _tourCommentService.Create(comment);
            return CreateResponse(result);
        }

        [HttpGet("comments")]
        public ActionResult<PagedResult<TourIssueCommentDto>> GetAllComments([FromQuery] long tourIssueReportId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var pagedResult = _tourCommentService.GetPaged(tourIssueReportId, page, pageSize);

            if (pagedResult == null)
                return CreateResponse(Result.Fail("No comments found"));

            var result = Result.Ok(pagedResult);
            return CreateResponse(result);
        }
    }
}
