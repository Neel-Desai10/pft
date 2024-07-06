using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface
{
    public interface ITransactionSummaryService
    {
        Task<TransactionSummaryResponseDto> GetTransactionSummary(int userId, int year, int month);
    }
}