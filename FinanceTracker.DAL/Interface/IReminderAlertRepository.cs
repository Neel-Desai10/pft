using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Interface;

public interface IReminderAlertRepository
{
     Task<List<ReminderAlertModel>> ReminderAlert();
}
