namespace FinanceTracker.DAL.Dtos.UserDto
{
    public class NotificationDto
    {
        // public int? UserId { get; set; }
        public int NotificationId { get; set; }
        public string FirstName { get; set; }
        public bool IsRead { get; set; }
        public string NotificationContent { get; set; }
        public string EmailId { get; set; }
        public DateTime? CreatedDate { get; set;}
    }
}