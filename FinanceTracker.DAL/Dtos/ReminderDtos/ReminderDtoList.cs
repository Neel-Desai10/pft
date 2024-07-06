using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceTracker.DAL.Dtos.ReminderDtos
{
    public class ReminderDtoList
    {
        public List<ReminderDateCountDto> ReminderCountData { get; set; }
    }
}