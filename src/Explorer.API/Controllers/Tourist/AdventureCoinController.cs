using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.UseCases.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/notifications")]
    [ApiController]
    public class AdventureCoinNotificationController : ControllerBase
    {
        private readonly IAdventureCoinNotificationService _notificationService;

        public AdventureCoinNotificationController(IAdventureCoinNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public ActionResult<List<ActivationCoinNotificationDto>> GetNotifications()
        {
            var userId = User.PersonId(); // Get the user ID

            var notifications = _notificationService.GetNotificationsForTourist(userId);

            if (notifications == null || !notifications.Any())
            {
                return NoContent(); // No notifications found
            }

            return Ok(notifications); // Return notifications
        }

        // Method to mark a single notification as read
        [HttpPost("mark-as-read/{notificationId}")]
        public IActionResult MarkNotificationAsRead(long notificationId)
        {
            try
            {
                _notificationService.MarkNotificationAsRead(notificationId);
                return NoContent();  // No content to return after marking as read
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Notification not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the notification.");
            }
        }

        // New method to mark all notifications as read
        [HttpPost("mark-all-as-read")]
        public IActionResult MarkAllNotificationsAsRead()
        {
            try
            {
                var userId = User.PersonId();  // Get user ID from the JWT token
                _notificationService.MarkAllNotificationsAsRead(userId);
                return NoContent();  // No content returned after marking all as read
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while marking all notifications as read.");
            }
        }
    }
}
