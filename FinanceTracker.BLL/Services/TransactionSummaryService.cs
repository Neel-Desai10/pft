using FinanceTracker.BLL.Interface;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Interface;
using FinanceTracker.DAL.Utility.Enum;

namespace FinanceTracker.BLL.Services
{
    public class TransactionSummaryService : ITransactionSummaryService
    {
        private readonly ITransactionSummaryRepository _transactionSummaryRepository;

        public TransactionSummaryService(ITransactionSummaryRepository transactionSummaryRepository)
        {
            _transactionSummaryRepository = transactionSummaryRepository;
        }

        public async Task<TransactionSummaryResponseDto> GetTransactionSummary(int userId, int year, int month)
        {
            var transactions = await _transactionSummaryRepository.GetTransactionsByMonthYear(userId, year, month);

            var groupedTransactions = transactions.GroupBy(t => new { t.CategoryId, t.Category.CategoryTypeId })
                .Select(g => new TotalTransactionResponseDto
                {
                    CategoryTypeId = g.Key.CategoryTypeId,
                    CategoryId = g.Key.CategoryId,
                    TotalAmount = g.Sum(t => t.Amount),
                    TotalCount = g.Count()
                }).ToList();

            var totalExpense = groupedTransactions.Where(t => t.CategoryTypeId == (int)CategoryTypeEnum.Expense).Sum(t => t.TotalAmount);
            var totalIncome = groupedTransactions.Where(t => t.CategoryTypeId == (int)CategoryTypeEnum.Income).Sum(t => t.TotalAmount);
            var availableBalance = totalIncome - totalExpense;

            return new TransactionSummaryResponseDto
            {
                TotalTransactionData = groupedTransactions,
                TotalExpense = totalExpense,
                TotalIncome = totalIncome,
                AvailableBalance = availableBalance
            };
        }
    }
}