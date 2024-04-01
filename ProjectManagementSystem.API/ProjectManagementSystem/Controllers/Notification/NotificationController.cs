using Domain.Models.Dtos.Auth.Response;
using Domain.Models.Dtos.UserNotification.Response;
using Domain.Models.ServiceResponses.Organization;
using Domain.Models.ServiceResponses.UserNotification;
using Domain.Services.ApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementSystem.Controllers.Notification
{
    [Route("user/notifications")]
    [ApiController]
    public class NotificationController(IUserNotificationService notificationService) : ControllerBase
    {

        private readonly IUserNotificationService _notificationService = notificationService;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse = await _notificationService.GetUserNotifications(userId);

            List<NotificationForResponseDto> notifications = [];

            foreach (var n in serviceResponse.Notifications)
            {
                notifications.Add(new()
                {
                    Id = n.Id.ToString(),
                    Title = n.Title,
                    Description = n.Description,
                    Type = n.Type,
                    Issuer = n.Issuer
                });
            }

            if (serviceResponse.Status.Equals(GetNotificationsServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new GetNotificationResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new GetNotificationResponse
            {
                Message = serviceResponse.Status,
                Notificatins = notifications
            });
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value;

            var serviceResponse= await _notificationService.DeleteAllUserNotifications(userId);

            if (serviceResponse.Status.Equals(GetNotificationsServiceResponseStatus.InternalError))
                return StatusCode(StatusCodes.Status500InternalServerError,
                     new DeleteNotificationResponse
                     {
                         Message = serviceResponse.Status
                     });

            return Ok(new DeleteNotificationResponse
            {
                Message = serviceResponse.Status
            });
        }
    }
}
