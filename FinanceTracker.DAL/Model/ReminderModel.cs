namespace FinanceTracker.DAL.Model
{
    public class ReminderModel
    {
        public int ReminderId { get; set; }
        public decimal Value { get; set; }
        public string Title { get; set; }
        public DateTime ReminderDate { get; set; }
        public byte ReminderAlertId { get; set; }
        public string? Notes { get; set; }
        public DateTime? EmailSentTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}