using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceTracker.DAL.Dtos
{
    public class ReminderDetailDto
    {
        public int ReminderId { get; set; }
        public decimal Value { get; set; }
        public string Title { get; set; }
        public byte ReminderAlertId { get; set; }
        public string? ReminderAlert { get; set; }
        public string ReminderTime { get; set; }
        public string? Notes { get; set; }


    }
}