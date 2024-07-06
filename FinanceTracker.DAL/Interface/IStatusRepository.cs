namespace FinanceTracker.DAL.Interface;

public interface IStatusRepository
{
    Task<List<StatusModel>> GetStatus();
}
