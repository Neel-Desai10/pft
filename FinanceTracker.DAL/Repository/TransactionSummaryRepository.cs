using FinanceTracker.DAL.Data;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Model;
using FinanceTracker.DAL.Resources;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DAL.Repository
{
    public class TransactionSummaryRepository : ITransactionSummaryRepository
    {
        private readonly ApplicationDbContext _transactionSummaryDbContext;

        public TransactionSummaryRepository(ApplicationDbContext transactionSummaryDbContext)
        {
            _transactionSummaryDbContext = transactionSummaryDbContext;
        }

        public async Task<List<TransactionModel>> GetTransactionsByMonthYear(int userId, int year, int month)
        {
            return await _transactionSummaryDbContext.Transactions
            .Where(t => t.CreatedBy == userId
                    && t.TransactionDate.Year == year 
                    && (month == ResponseResources.Zero || t.TransactionDate.Month == month) 
                    && t.DeletedDate == null)
                .Include(t => t.Category)
                .ThenInclude(c => c.CategoryType)
                .ToListAsync();
        }
    }
}