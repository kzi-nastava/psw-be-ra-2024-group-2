using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User;

[Authorize(Policy = "allLoggedPolicy")]
[Route("api/tourNotifications")]
public class TourIssueNotificationController : BaseApiController
{
    private readonly ITourIssueNotificationService _notificationService;

    public TourIssueNotificationController(ITourIssueNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPut]
    public ActionResult<TourIssueNotificationDto> Create([FromBody] TourIssueNotificationDto dto)
    {
        var result = _notificationService.Create(dto);
        return CreateResponse(result);
    }
    [HttpGet("{userId}")]
    public ActionResult<TourIssueNotificationDto> GetForUserId(long userId)
    {
        var result = _notificationService.GetForUserId(userId);
        return CreateResponse(result);
    }
    [HttpPut("markAsRead")]
    public ActionResult MarkAsRead([FromBody] MarkAsReadDto dto)
    {
        _notificationService.ReadNotifications(dto.UserId, dto.TourIssueReportId);
        return NoContent();
    }

    [HttpPut("markAllAsRead/{userId}")]
    public ActionResult MarkAllAsRead(long userId)
    {
        _notificationService.ReadAllNotifications(userId);
        return NoContent();
    }
}