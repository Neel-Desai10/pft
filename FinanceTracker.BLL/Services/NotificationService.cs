using FinanceTracker.BLL.Interface;
using FinanceTracker.BLL.shared.Enum;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AdminDto;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<AddNotificationDto> AddNotification(int UserId)
        {
            var notification = new AddNotificationDto()
            {
                NotificationContentId = (int)NotificationContentEnum.PendingRequest,
                IsRead = false,
                CreatedBy = UserId,
                CreatedDate = DateTime.Now,
                DeletedBy = (int)RolesEnum.Admin,
                DeletedDate = DateTime.Now.AddDays(7),
            };
            await _notificationRepository.AddNotification(notification);
            return notification;
        }
        public async Task<NotificationListDto> GetNotification()
        {
            var notifications = new NotificationListDto();
            notifications.NotificationList = await _notificationRepository.ReadNotificationDetails();
            notifications.UnreadNotificationCount = await _notificationRepository.GetUnreadNotificationCount();
            return notifications;
        }
        public async Task<NotificationStatusDto> UpdateUnreadNotificationList(NotificationStatusDto notificationStatus,int loggedInUserId)
        {
            var notifications = await _notificationRepository.UpdateUnreadNotification(notificationStatus);
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ModifiedBy = loggedInUserId;
                notification.ModifiedDate = DateTime.Now;
                await _notificationRepository.UpdateNotification(notification);
            }
            return notificationStatus;
        }
    }
}









