using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "allLoggedPolicy")]
    [Route("api/tourIssueReportView")]
    public class TourIssueReportViewController : BaseApiController
    {
        private readonly ITourIssueReportService _tourIssueReportService;
        private readonly ITourService _tourService;
        public TourIssueReportViewController(ITourIssueReportService tourIssueReportService, ITourService tourService)
        {
            _tourIssueReportService = tourIssueReportService;
            _tourService = tourService;
        }

        [HttpGet("{userId}")]
        public ActionResult<PagedResult<TourIssueReportDto>> GetAll(long userId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourIssueReportService.GetPaged(userId, page, pageSize);
            return CreateResponse(result);
        }
        [HttpGet("tourIssueReport/{tourIssueReportId}")]
        public ActionResult<TourDto> GetTourById(int tourIssueReportId)
        {
            var tourIssueReport = _tourIssueReportService.Get(tourIssueReportId);
            return CreateResponse(tourIssueReport);
        }


        [HttpGet("tour/{tourId}")]
        public ActionResult<TourDto> GetTourById(long tourId)
        {
            var tour = _tourService.GetById(tourId);
            return CreateResponse(tour);
        }
    }
}
