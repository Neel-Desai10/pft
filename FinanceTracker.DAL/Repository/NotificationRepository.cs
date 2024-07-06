using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.UserDto;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Mapper;
using FinanceTracker.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _userContext;
        public NotificationRepository(ApplicationDbContext userContext)
        {
            _userContext = userContext;
        }
        public async Task<int> GetUnreadNotificationCount()
        {
            var notificationCount = await _userContext.Notifications.CountAsync(n => n.IsRead == false);
            return notificationCount;
        }
        public async Task<NotificationModel> AddNotification(AddNotificationDto addNotificationDto)
        {
            var data = addNotificationDto.NotificationDtoMapper();
            await _userContext.AddAsync(data);
            await _userContext.SaveChangesAsync();
            return data;
        }
        public async Task<List<NotificationModel>> UpdateUnreadNotification(NotificationStatusDto notificationStatus)
        {
            var unreadNotifications = _userContext.Notifications.Where(x => x.IsRead == false).ToList();
            return unreadNotifications;
        }
        public async Task UpdateNotification(NotificationModel notificationModel)
        {
            _userContext.UpdateRange(notificationModel);
            await _userContext.SaveChangesAsync();
        }
        public async Task<List<NotificationDto>> ReadNotificationDetails()
        {
            var currentDateTime = DateTime.Now;
            var notificationList = await _userContext.Notifications
                .Where(n => currentDateTime <= n.DeletedDate)
                .OrderByDescending(x => x.CreatedDate)
                .Join(_userContext.UserData,
                    notification => notification.CreatedBy,
                    user => user.UserId,
                    (notification, user) => new NotificationDto
                    {
                        FirstName = user.FirstName,
                        NotificationId = notification.NotificationId,
                        IsRead = notification.IsRead,
                        NotificationContent = _userContext.NotificationContents
                                                .Where(x => x.NotificationContentId == notification.NotificationContentId)
                                                .Select(x => x.NotificationContent)
                                                .FirstOrDefault() + user.FirstName,
                        EmailId = user.EmailId,
                        CreatedDate = notification.CreatedDate
                    })
                .ToListAsync();
            return notificationList;
        }
    }
}