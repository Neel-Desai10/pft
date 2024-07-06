using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Model;

namespace FinanceTracker.DAL.Interface
{
    public interface IIncomeExpenseRepository
    {
        Task<int> GetTotalUsersCount(int userId, byte CategoryTypeId, int month, int year);
        Task<List<TransactionDto>> GetTransactionsByCategoryType(int userId, byte CategoryTypeId, PaginationParameters paginationParameters, int month, int year);
        Task<List<UserTransactionDto>> UserTransactions(int createdBy, PaginationParameters paginationParameters, int month, int year);
        Task<bool> CheckCategoryByCategoryId(int category, int categoryType);
        Task AddIncomeExpense(IncomeExpenseRequestDto incomeExpenseDto, int userId);
        Task<int> GetTotalUserTransaction(int userId, int month, int year);
        Task<List<UserTransactionDto>> UserTransactionData(int userId, int month, int year);
        Task DeleteIncomeExpense(TransactionModel transactionModel);
        Task<TransactionModel> GetTransactionById(int userId,int transactionId);
        Task<TransactionModel> GetIncomeExpenseById(int transactionId,int loggedInUserId);
        Task UpdateIncomeExpense(TransactionModel transactionModel);
    }
}