using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Mapper
{
    public static class NotificationMapper
    {
        public static NotificationModel NotificationDtoMapper(this AddNotificationDto notification)
        {
            return new NotificationModel{
                NotificationContentId = notification.NotificationContentId,
                IsRead = notification.IsRead,
                CreatedBy = notification.CreatedBy,
                CreatedDate = notification.CreatedDate,
                DeletedBy = notification.DeletedBy,
                DeletedDate = notification.DeletedDate
            };
        }
    }
}