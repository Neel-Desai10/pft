namespace FinanceTracker.DAL.Dtos
{
    public class AddNotificationDto
    {
        public byte NotificationContentId { get; set; }
        public bool IsRead { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}