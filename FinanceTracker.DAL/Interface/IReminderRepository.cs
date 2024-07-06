using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Dtos.ReminderDtos;
using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Interface
{
    public interface IReminderRepository
    {
        Task<List<ReminderDateTimeDto>> GetReminderDetails(int userId, int month, int year);
        Task<ReminderModel> CreateReminder(int userId, ReminderDto reminderResponse);
        Task<UserModel> GetUserById(int userId);
        Task<ReminderDetailDto> GetReminderDetailsById(int userId,int reminderId);
        Task<List<ReminderListDto>> DateWiseReminderList(int userId, int month, int year, int date);
        Task<ReminderModel> GetReminderByReminderId(int userId, int reminderId);
        Task<ReminderModel> UpdateReminderByUserId(ReminderModel reminders);
        Task DeleteReminderByReminderId(ReminderModel reminder);
        Task<List<ReminderModel>> CheckMailSend();
        Task MailSent(int reminderId);
    }
}