using FinanceTracker.DAL.Dtos.UserDto;

namespace FinanceTracker.DAL.Dtos.AdminDto
{
    public class NotificationListDto
    {
        public List<NotificationDto> NotificationList { get; set; }
        public int UnreadNotificationCount {get; set;}
    }
}