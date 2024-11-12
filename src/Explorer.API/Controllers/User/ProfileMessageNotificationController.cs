using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.User
{
    [Authorize(Policy = "allLoggedPolicy")]
    [Route("api/profile/messaging")]
    public class ProfileMessageNotificationController : BaseApiController
    {
        private readonly IProfileMessageNotificationService _notificationService;

        public ProfileMessageNotificationController(IProfileMessageNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{userId}")]
        public ActionResult<ProfileMessageNotificationDto> GetForUserId(long userId)
        {
            var result = _notificationService.GetForUserId(userId);
            return CreateResponse(result);
        }

        [HttpPut("mark/all/read/{userId}")]
        public ActionResult MarkAllAsRead(long userId)
        {
            _notificationService.ReadAllNotifications(userId);
            return NoContent();
        }

        [HttpPut("mark/read/{notificationId}")]
        public ActionResult MarkAsRead(long userId, long notificationId)
        {
            _notificationService.ReadNotification(userId, notificationId);
            return NoContent();
        }
    }
}
