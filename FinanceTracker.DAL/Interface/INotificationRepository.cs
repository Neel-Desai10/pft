using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Interface
{
    public interface INotificationRepository
    {
        Task<NotificationModel> AddNotification(AddNotificationDto addNotificationDto);
        Task<int> GetUnreadNotificationCount();
        Task<List<NotificationModel>> UpdateUnreadNotification(NotificationStatusDto notificationStatusDto);
        Task UpdateNotification(NotificationModel notificationModel);
        Task<List<NotificationDto>> ReadNotificationDetails();
    }
}