using FinanceTracker.DAL.Dtos.ReminderDtos;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Model;

namespace FinanceTracker.BLL.Interface
{
    public interface IReminderService
    {
        Task<ReminderDtoList> MonthWiseReminder(int userId,int month , int year);
        Task<string> CreateReminder(int userId,ReminderDto reminderResponse);
        Task<ReminderDetailDto> GetReminderDetailsById(int userId,int reminderId);
        Task<ReminderResponseDto> DateWiseReminder(int userId, int month, int year, int date);
        Task<string> UpdateReminder(int userId, int reminderId, ReminderDto reminderResponse);
        Task<string> DeleteReminder(int userId, int reminderId);
        
    }
}
