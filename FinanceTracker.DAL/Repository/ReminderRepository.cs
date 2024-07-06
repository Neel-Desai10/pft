using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Dtos.ReminderDtos;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Model;
using FinanceTracker.DAL.Resources;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly ApplicationDbContext _reminderContext;
        public ReminderRepository(ApplicationDbContext reminderContext)
        {
            _reminderContext = reminderContext;
        }
        public async Task<ReminderModel> UpdateReminderByUserId(ReminderModel reminders)
        {
            _reminderContext.Update(reminders);
            await _reminderContext.SaveChangesAsync();
            return reminders;
        }
        public async Task<ReminderModel> CreateReminder(int userId, ReminderDto reminderResponse)
        {
            var reminder = new ReminderModel
            {
                Value = reminderResponse.Value,
                Title = reminderResponse.Title,
                ReminderDate = reminderResponse.ReminderDate,
                ReminderAlertId = reminderResponse.ReminderAlertId,
                Notes = reminderResponse.Notes,
                CreatedDate = DateTime.Now,
                CreatedBy = userId
            };
            await _reminderContext.AddAsync(reminder);
            await _reminderContext.SaveChangesAsync();
            return reminder;
        }
        public async Task<UserModel> GetUserById(int userId)
        {
            var user = await _reminderContext.UserData.FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive == true);
            return user;
        }
        public async Task<ReminderDetailDto> GetReminderDetailsById(int userId, int reminderId)
        {
            var reminderDetail = await _reminderContext.Reminders.FirstOrDefaultAsync(x => x.ReminderId == reminderId && x.CreatedBy == userId);

            if (reminderDetail is null)
            {
                return new ReminderDetailDto();
            }
            var reminderAlert = await _reminderContext.ReminderAlerts
                    .Where(x => x.ReminderAlertId == reminderDetail.ReminderAlertId)
                    .Select(x => x.ReminderAlert)
                    .FirstOrDefaultAsync();

            return new ReminderDetailDto
            {
                ReminderId = reminderDetail.ReminderId,
                Value = reminderDetail.Value,
                Title = reminderDetail.Title,
                ReminderTime = DateTime.Today.Add(reminderDetail.ReminderDate.TimeOfDay).ToString(ResponseResources.ReminderTimeFormat),
                ReminderAlertId = reminderDetail.ReminderAlertId,
                ReminderAlert = reminderAlert,
                Notes = reminderDetail.Notes,
            };
        }

        public async Task<List<ReminderListDto>> DateWiseReminderList(int userId, int month, int year, int date)
        {
            var reminderList = await _reminderContext.Reminders
                .Where(n => n.ReminderDate.Day == date && n.ReminderDate.Month == month && n.ReminderDate.Year == year && n.CreatedBy == userId && n.DeletedDate == null)
                .OrderBy(x => x.ReminderDate.TimeOfDay)
                .Select(reminder => new ReminderListDto
                {
                    ReminderId = reminder.ReminderId,
                    Title = reminder.Title,
                    ReminderTime = DateTime.Today.Add(reminder.ReminderDate.TimeOfDay).ToString(ResponseResources.ReminderTimeFormat),
                })
                .ToListAsync();
            return reminderList;
        }

        public async Task<List<ReminderDateTimeDto>> GetReminderDetails(int userId, int month, int year)
        {
            return await _reminderContext.Reminders
                                         .Where(k => k.CreatedBy == userId && k.ReminderDate.Month == month && k.ReminderDate.Year == year && k.DeletedDate == null)
                                         .Select(d => new ReminderDateTimeDto
                                         {
                                             ReminderDateTime = d.ReminderDate,
                                         })
                                         .OrderBy(x => x.ReminderDateTime)
                                            .ToListAsync();
        }

        public async Task<ReminderModel> GetReminderByReminderId(int userId, int reminderId)
        {
            var reminder = await _reminderContext.Reminders.FirstOrDefaultAsync(x => x.ReminderId == reminderId && x.CreatedBy == userId && x.DeletedDate == null);
            return reminder;
        }
        public async Task DeleteReminderByReminderId(ReminderModel reminder)
        {
            _reminderContext.Update(reminder);
            await _reminderContext.SaveChangesAsync();
        }
        public async Task<List<ReminderModel>> CheckMailSend()
        {
            return await _reminderContext.Reminders
                .Where(x => x.EmailSentTime == null && x.DeletedDate == null && (x.ReminderDate.Date == DateTime.Today || x.ReminderDate.Date == DateTime.Today.AddDays(1)))
                .ToListAsync();
        }
        public async Task MailSent(int reminderId)
        {
            var reminderDetails = await _reminderContext.Reminders.FirstOrDefaultAsync(x => x.ReminderId == reminderId);
            reminderDetails.EmailSentTime = DateTime.Now;
            _reminderContext.Update(reminderDetails);
            await _reminderContext.SaveChangesAsync();
        }
    }
}