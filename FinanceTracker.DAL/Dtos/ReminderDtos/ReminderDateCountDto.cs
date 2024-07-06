using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceTracker.DAL.Dtos.ReminderDtos
{
    public class ReminderDateCountDto
    {
        public DateOnly ReminderDate { get; set; }

        public int ReminderCount { get; set; }
    }
}