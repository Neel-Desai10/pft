using FinanceTracker.DAL.Dtos;

namespace FinanceTracker.BLL.Interface
{
    public interface IIncomeExpenseService
    {
        Task<IncomeExpenseDto> GetTransactionsByCategoryType(int userId, byte CategoryTypeId, PaginationParameters paginationParameters, int month, int year);
        Task<TransactionListDto> TransactionResponse(int userId, PaginationParameters paginationParameters, int month, int year);
        Task<string> AddIncomeExpense(IncomeExpenseRequestDto incomeExpenseRequestDto, int categoryTypeId, int userId);
        Task<ExportTransactionDto> TransactionDataToPdf(int userId, int month, int year);
        Task<string> DeleteIncomeExpense(int loginUserId,int userId,int transactionId);
        Task<string> UpdateIncomeExpense(int transactionId, int loginUserId,int userId ,IncomeExpenseRequestDto incomeExpenseRequestDto);
    }
}