using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Authorize(Policy = "allLoggedPolicy")]
[Route("api/tournotifiactions")]
public class TourIssueNotificationController : BaseApiController
{
    private readonly ITourIssueNotificationService _commentService;

    public TourIssueNotificationController(TourIssueNotificationService notificationService)
    {
        _commentService = notificationService;
    }

    [HttpPut]
    public ActionResult<TourIssueNotificationDto> Create([FromBody] TourIssueNotificationDto dto)
    {
        var result = _commentService.Create(dto);
        return CreateResponse(result);
    }
    [HttpGet]
    public ActionResult<TourIssueNotificationDto> GetByUserId(long userId)
    {
        var result = _commentService.GetByUserId(userId);
        return CreateResponse(result);
    }
}