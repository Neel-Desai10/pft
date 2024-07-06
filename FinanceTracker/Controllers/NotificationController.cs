using System.ComponentModel.DataAnnotations;
using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Resources;
using FinanceTracker.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;
        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService, IUserDetailsService userDetailsService) : base (userDetailsService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }
        [HttpGet("admin/notification")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetNotification()
        {
            try
            {
                GetUserId();
                var result = await _notificationService.GetNotification();
                _logger.LogInformation(ResponseResources.NotificationFetchSuccess);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseResources.NotificationError);
                return Error(ex.Message.ToString());
            }
        }
        [HttpPut("admin/update-notification")]
        public async Task<IActionResult> UpdateNotification([Required][FromQuery] NotificationStatusDto notificationStatus)
        {
            try
            {
                var loggedInUserId = GetUserId();
                var result = await _notificationService.UpdateUnreadNotificationList(notificationStatus,loggedInUserId);
                _logger.LogInformation(ResponseResources.NotificationIsReadUpdated);
                return Ok(ValidationResources.NotificationUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseResources.NotificationNotUpdated);
                return Error(ex.Message.ToString());
            }
        }
    }
}
