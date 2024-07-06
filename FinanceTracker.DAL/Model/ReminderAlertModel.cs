using System.ComponentModel.DataAnnotations;
namespace FinanceTracker.DAL.Model
{
    public class ReminderAlertModel
    {
        [Key]
        public byte ReminderAlertId { get; set; }
        public string ReminderAlert { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}