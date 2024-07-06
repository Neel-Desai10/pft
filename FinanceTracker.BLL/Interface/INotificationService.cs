using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.AdminDto;

namespace FinanceTracker.BLL.Interface
{
    public interface INotificationService
    {
        Task<AddNotificationDto> AddNotification(int UserId);
        Task<NotificationListDto> GetNotification();
        Task<NotificationStatusDto> UpdateUnreadNotificationList(NotificationStatusDto notificationStatus,int logInUserId);
    }
}