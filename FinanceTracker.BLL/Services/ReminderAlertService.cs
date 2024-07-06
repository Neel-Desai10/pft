using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;

namespace FinanceTracker.BLL.Services;

public class ReminderAlertService : IReminderAlertService
{
    private readonly IReminderAlertRepository _reminderAlertRepository;
    public ReminderAlertService(IReminderAlertRepository reminderAlertRepository)
    {
        _reminderAlertRepository = reminderAlertRepository;
    }

    public async Task<ReminderAlertDto> GetAllReminderAlert()
    {
        var reminderAlert = new ReminderAlertDto();
        reminderAlert.ReminderAlertList = await _reminderAlertRepository.ReminderAlert();
        return reminderAlert;
    }
}
