using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers
{
    [Authorize(Policy = "allLoggedPolicy")]
    [Route("api/tourIssueComment")]
    public class TourIssueCommentController : BaseApiController
    {
        private readonly ITourIssueCommentService _tourIssueCommentService;
        private readonly ITourIssueReportService _tourIssueReportService;

        public TourIssueCommentController(ITourIssueCommentService tourIssueCommentService, ITourIssueReportService tourIssueReportService)
        {
            _tourIssueCommentService = tourIssueCommentService;
            _tourIssueReportService = tourIssueReportService;
        }

        [HttpPost("comment")]
        public ActionResult<TourIssueCommentDto> CreateComment([FromBody] TourIssueCommentDto comment)
        {
            var result = _tourIssueCommentService.CreateComment(comment);
            return CreateResponse(result);
        }

        [HttpGet("comments")]
        public ActionResult<PagedResult<TourIssueCommentDto>> GetAllComments([FromQuery] long tourIssueReportId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var pagedResult = _tourIssueCommentService.GetPaged(tourIssueReportId, page, pageSize);

            if (pagedResult.TotalCount == 0)
                return CreateResponse(Result.Fail(FailureCode.NotFound).WithError("no comments found"));

            var result = Result.Ok(pagedResult);
            return CreateResponse(result);
        }
    }
}
