using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository;

public class ReminderAlertRepository : IReminderAlertRepository
{
    private readonly ApplicationDbContext _reminderAlertContext;
    public ReminderAlertRepository(ApplicationDbContext reminderAlertContext)
    {
        _reminderAlertContext = reminderAlertContext;
    }
    public async Task<List<ReminderAlertModel>> ReminderAlert()
    {
        return await _reminderAlertContext.ReminderAlerts.ToListAsync();
    }
}
