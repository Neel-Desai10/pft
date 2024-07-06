using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Interface
{
    public interface ITransactionSummaryRepository
    {
        Task<List<TransactionModel>> GetTransactionsByMonthYear(int userId, int year, int month);
    }
}