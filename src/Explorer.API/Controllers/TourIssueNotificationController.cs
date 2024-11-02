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
    private readonly ITourIssueNotificationService _notificationService;

    public TourIssueNotificationController(TourIssueNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPut]
    public ActionResult<TourIssueNotificationDto> Create([FromBody] TourIssueNotificationDto dto)
    {
        var result = _notificationService.Create(dto);
        return CreateResponse(result);
    }
    [HttpGet]
    public ActionResult<TourIssueNotificationDto> GetForUserId(long userId)
    {
        var result = _notificationService.GetForUserId(userId);
        return CreateResponse(result);
    }
}