using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface;

public interface IReminderAlertService
{
    Task<ReminderAlertDto> GetAllReminderAlert();
}
